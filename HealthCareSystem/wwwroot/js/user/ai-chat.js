// AI Chat functionality
const aiMessages = []
let isTyping = false

document.addEventListener("DOMContentLoaded", () => {
    const userId = document.getElementById("userId").innerText;  // Lấy UserId từ Razor Page
    if (userId) {
        getMessages(userId);  // Lấy tin nhắn khi tải trang
    }
    updateUserInfo()
    loadRecommendedDoctors()
})

// Load recommended doctors (example)
function loadRecommendedDoctors() {
    const doctors = [
        {
            name: "Dr. Sarah Johnson",
            specialty: "Cardiology",
            experience: "15 years experience",
            avatar: "/placeholder.svg?height=60&width=60",
            availability: "Available Today",
        },
        {
            name: "Dr. Michael Chen",
            specialty: "Dermatology",
            experience: "12 years experience",
            avatar: "/placeholder.svg?height=60&width=60",
            availability: "Available Tomorrow",
        },
    ]

    const container = document.getElementById("recommendedDoctors")
    container.innerHTML = doctors
        .map(
            (doctor) => `
    <div class="recommended-doctor-card">
      <div class="doctor-card-header">
        <img src="${doctor.avatar}" alt="${doctor.name}" class="doctor-card-avatar">
        <div class="doctor-card-info">
          <h6>${doctor.name}</h6>
          <p>${doctor.specialty}</p>
          <span class="doctor-experience">${doctor.experience}</span>
        </div>
      </div>
      <div class="doctor-card-footer">
        <span class="availability-badge">${doctor.availability}</span>
        <button class="btn btn-sm btn-primary" onclick="bookWithDoctor('${doctor.name}')">Book</button>
      </div>
    </div>
  `
        )
        .join("")
}

// Gửi tin nhắn (POST request)
function sendAIMessage(event) {
    event.preventDefault()

    const input = document.getElementById("aiMessageInput")
    const messageText = input.value.trim()

    if (!messageText || isTyping) return

    // Thêm tin nhắn của người dùng
    addMessage("user", messageText)

    // Xóa input
    input.value = ""

    // Hiển thị biểu tượng đang gõ
    showTypingIndicator()

    // Gửi tin nhắn đến API (POST request)
    const userId = document.getElementById("userId").innerText;  // Lấy UserId từ Razor Page
    console.log("UserId from Razor Page:", userId);
    fetch("/api/chatbox/message", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            UserId: userId,  // Thay đổi UserId thực tế từ session hoặc localStorage
            Message: messageText
        })
    })
        .then(response => response.json())
        .then(data => {
            hideTypingIndicator();
            addMessage("ai", data.aiReply);  // Hiển thị phản hồi từ AI
        })
        .catch(error => {
            hideTypingIndicator();
            console.error("Error sending message:", error);
            addMessage("ai", "Sorry, there was an error. Please try again later.");
        });
}

// Thêm tin nhắn vào giao diện
// Thêm tin nhắn vào giao diện
function addMessage(sender, text) {
    const messagesContainer = document.getElementById("aiChatMessages");
    const time = new Date().toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });

    const messageElement = document.createElement("div");
    messageElement.className = `message ${sender === "user" ? "user-message" : "ai-message"}`;

    // Format tin nhắn AI trước khi thêm vào giao diện
    const formattedText = formatMessageText(text);  // Sử dụng hàm formatMessageText cho mọi tin nhắn

    messageElement.innerHTML = `
        <div class="message-avatar">
            ${sender === "user"
            ? '<img src="/placeholder.svg?height=36&width=36" alt="User">'
            : '<div class="ai-avatar-small"><i class="fas fa-robot"></i></div>'}
        </div>
        <div class="message-content">
            <div class="message-header">
                <span class="message-sender">${sender === "user" ? "You" : "AI Assistant"}</span>
                <span class="message-time">${time}</span>
            </div>
            <div class="message-text">${formattedText}</div>
            ${sender === "ai" ? ` 
                <div class="ai-disclaimer">
                    <i class="fas fa-exclamation-triangle"></i>
                    This AI assistant provides general information only and should not replace professional medical advice.
                </div>
            ` : ""}
        </div>
    `;

    messagesContainer.appendChild(messageElement);
    messagesContainer.scrollTop = messagesContainer.scrollHeight; // Scroll đến cuối tin nhắn
}

// Hàm định dạng tin nhắn và xóa các dấu *
function formatMessageText(responseText) {
    if (!responseText) return "";

    // Loại bỏ các dấu * trước các mục và chuyển thành các phần tử danh sách <ul><li>
    let formattedResponse = responseText.split("\n").map(line => {
        // Nếu dòng bắt đầu bằng *, chuyển thành <li> item
        if (line.startsWith("*")) {
            return `<li>${line.replace("*", "").trim()}</li>`;  // Loại bỏ dấu * và chuyển thành <li>
        } else {
            return `<p>${line.trim()}</p>`;  // Các dòng không có dấu * thì tạo thành đoạn văn
        }
    }).join(""); // Ghép các phần tử lại với nhau

    return `<ul>${formattedResponse}</ul>`;  // Bao toàn bộ thành danh sách <ul>
}

// Hiển thị biểu tượng đang gõ
function showTypingIndicator() {
    if (isTyping) return

    isTyping = true
    const messagesContainer = document.getElementById("aiChatMessages")

    const typingElement = document.createElement("div")
    typingElement.className = "ai-typing-indicator"
    typingElement.id = "typingIndicator"

    typingElement.innerHTML = `
    <div class="message-avatar">
      <div class="ai-avatar-small">
        <i class="fas fa-robot"></i>
      </div>
    </div>
    <div class="typing-content">
      <span class="typing-text">AI is typing</span>
      <div class="typing-dots">
        <span></span>
        <span></span>
        <span></span>
      </div>
    </div>
  `

    messagesContainer.appendChild(typingElement)
    messagesContainer.scrollTop = messagesContainer.scrollHeight
}

// Ẩn biểu tượng đang gõ
function hideTypingIndicator() {
    isTyping = false
    const typingElement = document.getElementById("typingIndicator")
    if (typingElement) {
        typingElement.remove()
    }
}

// Lấy tin nhắn từ API (GET request)
function getMessages(userId) {
    fetch(`/api/chatbox/messages/${userId}`)
        .then((response) => response.json())
        .then((data) => {
            data.forEach((message) => {
                const formattedMessage = formatMessageText(message.content);  // Định dạng tin nhắn từ DB
                addMessage(message.sender.toLowerCase(), formattedMessage); // Hiển thị tất cả tin nhắn
            });
        })
        .catch((error) => {
            console.error("Error fetching messages:", error);
        });
}

// Khởi tạo trả lời từ AI (Ví dụ cho việc tự động phản hồi)
function generateAIResponse(userMessage) {
    const responses = {
        flu: "Common flu symptoms include fever, body aches, fatigue, cough, and congestion. Rest, hydration, and over-the-counter medications can help manage symptoms. Consult a doctor if symptoms worsen or persist.",
        "blood pressure":
            "To manage high blood pressure: reduce sodium intake, exercise regularly, maintain a healthy weight, limit alcohol, quit smoking, and manage stress. Regular monitoring and medication compliance are important.",
        diabetes:
            "For diabetes management, focus on: complex carbohydrates, lean proteins, healthy fats, plenty of vegetables, and portion control. Avoid sugary drinks and processed foods. Regular blood sugar monitoring is essential.",
        water:
            "Generally, aim for 8 glasses (64 ounces) of water daily, but needs vary based on activity level, climate, and individual factors. Monitor your urine color - pale yellow indicates good hydration.",
        sleep:
            "Healthy sleep habits include: consistent sleep schedule, comfortable sleep environment, avoiding screens before bed, regular exercise, limiting caffeine, and creating a relaxing bedtime routine.",
        stress:
            "Natural stress management techniques: deep breathing exercises, regular physical activity, meditation, adequate sleep, healthy diet, social connections, and engaging in hobbies you enjoy.",
    }

    const lowerMessage = userMessage.toLowerCase()

    for (const [key, response] of Object.entries(responses)) {
        if (lowerMessage.includes(key)) {
            return response
        }
    }

    // Default responses
    const defaultResponses = [
        "Thank you for your question. Based on the information provided, I recommend consulting with a healthcare professional for personalized advice. In the meantime, maintaining a healthy lifestyle with proper diet, exercise, and adequate rest is always beneficial.",
        "I understand your concern. While I can provide general health information, it's important to discuss your specific situation with a qualified healthcare provider who can give you personalized recommendations.",
        "That's a great question about your health. For the most accurate and personalized advice, I'd recommend scheduling an appointment with one of our healthcare professionals. They can provide specific guidance based on your medical history.",
        "I'm here to help with general health information. For your specific situation, consulting with a healthcare provider would be the best approach to ensure you receive appropriate care and guidance.",
    ]

    return defaultResponses[Math.floor(Math.random() * defaultResponses.length)]
}

// Hỏi các câu hỏi nhanh từ giao diện
function askQuickQuestion(question) {
    document.getElementById("aiMessageInput").value = question
    sendAIMessage({ preventDefault: () => { } })
}

// Nhận dạng giọng nói
function startVoiceInput() {
    const webkitSpeechRecognition = window.webkitSpeechRecognition
    if (webkitSpeechRecognition) {
        const recognition = new webkitSpeechRecognition()
        recognition.continuous = false
        recognition.interimResults = false
        recognition.lang = "en-US"

        recognition.onstart = () => {
            document.querySelector(".ai-voice-btn i").className = "fas fa-microphone-slash"
        }

        recognition.onresult = (event) => {
            const transcript = event.results[0][0].transcript
            document.getElementById("aiMessageInput").value = transcript
        }

        recognition.onend = () => {
            document.querySelector(".ai-voice-btn i").className = "fas fa-microphone"
        }

        recognition.onerror = (event) => {
            console.error("Speech recognition error:", event.error)
            alert("Speech recognition error. Please try again.")
        }

        recognition.start()
    } else {
        alert("Speech recognition is not supported in your browser.")
    }
}

function bookWithDoctor(doctorName) {
    alert(`Booking appointment with ${doctorName}. Redirecting to appointments page...`)
    window.location.href = "appointments-modern.html"
}

function logout() {
    if (confirm("Are you sure you want to logout?")) {
        localStorage.clear()
        window.location.href = "index.html"
    }
}
