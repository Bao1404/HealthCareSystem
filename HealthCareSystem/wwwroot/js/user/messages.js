// Patient Messages functionality
let currentConversation = null
let conversations = []

document.addEventListener("DOMContentLoaded", () => {
    const userId = localStorage.getItem("patientId")
    console.log(userId);
    updateUserInfo()
    loadConversations()

    setupEventListeners()
})
let connection = null;


async function setupSignalR(conversationId) {
    if (connection) {
        // If already connected to SignalR, return early
        console.log("Already connected to SignalR");
        return;
    }

    // Create a new SignalR connection
    connection = new signalR.HubConnectionBuilder()
        .withUrl(`/chathub?conversationId=${conversationId}`)
        .build();

    // Listen for incoming messages from SignalR
    connection.on("ReceiveMessage", (senderId, message) => {
        console.log("📨 New message from:", senderId, ":", message);
        loadMessagesFromApi(conversationId); // Cập nhật UI
    });

    try {
        // Start the connection
        await connection.start();
        console.log("🟢 Connected to SignalR");
    } catch (err) {
        console.error("SignalR Error:", err);
    }
}

function updateUserInfo() {
    const userName = localStorage.getItem("userName") || "Patient User"
    document.getElementById("userName").textContent = userName
}

async function loadConversations() {
    const userId = localStorage.getItem("patientId")
    console.log("Loaded patientId:", userId)
    if (!userId) return

    try {
        const response = await fetch(`/api/ApiConversation/patient/${userId}`)
        if (!response.ok) throw new Error("Failed to load conversations")

        const data = await response.json()
        console.log("✅ Conversations data:", data) // ✅ In ra để kiểm tra

        conversations = data
        renderConversations()
    } catch (err) {
        console.error("❌ Error loading conversations:", err)
    }
}

function renderConversations() {
    const container = document.getElementById("conversationsList")
    container.innerHTML = conversations.map((conversation) => `
        <div class="conversation-item" onclick="selectConversation(${conversation.conversationId})">
            <div class="conversation-avatar">
                <img src="${conversation.doctorUser?.avatarUrl || '/placeholder.svg?height=48&width=48'}" alt="${conversation.doctorUser?.fullName || 'Doctor'}">
                <div class="status-indicator online"></div>
            </div>
            <div class="conversation-content">
                <div class="conversation-header">
                    <h6 class="conversation-name">${conversation.doctorUser?.fullName || 'Doctor'}</h6>
                    <span class="conversation-time">${new Date(conversation.updatedAt).toLocaleTimeString()}</span>
                </div>
                <p class="conversation-specialty">ID: ${conversation.doctorUser?.userId}</p>
                <p class="conversation-preview">Click to view messages</p>
            </div>
        </div>
    `).join("")
}

function selectConversation(conversationId) {
    currentConversation = conversations.find((c) => c.conversationId === conversationId)
    if (!currentConversation) return

    document.querySelectorAll(".conversation-item").forEach((item) => item.classList.remove("active"))
    event.currentTarget.classList.add("active")

    showChatInterface()
    loadMessagesFromApi(conversationId)
         setupSignalR(conversationId);
    console.log("📬 Selected conversation:", currentConversation)
}

async function loadMessagesFromApi(conversationId) {
    const container = document.getElementById("chatMessages")
    container.innerHTML = "<p>Loading...</p>"

    try {
        setupSignalR(conversationId); console.log("🔄 Loading messages for conversation:", conversationId)
        const res = await fetch(`/api/APIMessage/conversation/${conversationId}`)
        if (!res.ok) throw new Error("Failed to load messages")

        const messages = await res.json()
        container.innerHTML = messages.map((message) => {
            const sender = message.sender || {}
            const senderRole = sender.role || "Unknown"
            const senderName = sender.fullName || "Unknown"
            const senderAvatar = sender.avatarUrl || "/placeholder.svg?height=36&width=36"

            return `
                <div class="message ${senderRole === "Patient" ? "user-message" : "doctor-message"}">
                    <div class="message-avatar">
                        <img src="${senderAvatar}" alt="${senderName}">
                    </div>
                    <div class="message-content">
                        <div class="message-header">
                            <span class="message-sender">${senderRole === "Patient" ? "You" : senderName}</span>
                            <span class="message-time">${new Date(message.sentAt).toLocaleTimeString()}</span>
                        </div>
                        <div class="message-text">${message.content}</div>
                    </div>
                </div>
            `
        }).join("")

        container.scrollTop = container.scrollHeight
    } catch (err) {
        console.error("❌ Error loading messages:", err)

        container.innerHTML = "<p>Failed to load messages.</p>"
    }
}

function showChatInterface() {
    if (!currentConversation) return

    document.getElementById("chatHeader").style.display = "flex"
    document.getElementById("chatInputContainer").style.display = "block"
    document.getElementById("chatAvatar").src = currentConversation.doctorUser?.avatarUrl
    document.getElementById("chatDoctorName").textContent = currentConversation.doctorUser?.fullName
    document.getElementById("chatDoctorSpecialty").textContent = "Doctor"

    const emptyChat = document.querySelector(".empty-chat")
    if (emptyChat) emptyChat.style.display = "none"
}

async function sendMessage(event) {
    event.preventDefault();

    if (!currentConversation) return;

    const input = document.getElementById("messageInput");
    const messageText = input.value.trim();
    if (!messageText) return;

    const conversationId = currentConversation.conversationId;
    const senderId = parseInt(localStorage.getItem("patientId")); // 👈 role: bệnh nhân
    const receiverId = currentConversation.doctorUser?.userId;

    console.log("👤 Sender (patient):", senderId);
    console.log("📥 Receiver (doctor):", receiverId);
    console.log("🧵 Conversation:", conversationId);

    if (!senderId || !receiverId || !conversationId) {
        console.error("❌ Missing senderId, receiverId, or conversationId");
        return;
    }

    await setupSignalR(conversationId); // Khởi tạo kết nối SignalR nếu cần

    const newMessage = {
        conversationId: conversationId,
        senderId: senderId,
        messageType: "text",
        content: messageText
    };

    try {
        const res = await fetch("/api/APIMessage/send", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(newMessage)
        });

        if (!res.ok) throw new Error("Failed to send message");

        // Gửi socket thông báo tới bên còn lại
        await connection.invoke("SendMessage", conversationId.toString(), senderId.toString(), messageText);

        // Xóa input & cập nhật UI
        input.value = "";
        await loadMessagesFromApi(conversationId);

    } catch (err) {
        console.error("❌ Error sending message:", err);
    }
}


function logout() {
    if (confirm("Are you sure you want to logout?")) {
        localStorage.clear()
        window.location.href = "index.html"
    }
}
