using BusinessObjects;

namespace Repositories.IRepositories
{
    public interface IConversationRepository
    {
        Task<Conversation?> GetConversationById(int conversationId);
        Task<List<Conversation>> GetConversationsByDoctorId(int doctorId);
        Task<List<Conversation>> GetConversationsByPatientId(int patientId);
        Task<Conversation?> GetConversationByDoctorAndPatient(int doctorId, int patientId);
        Task CreateConversation(Conversation conversation);
        Task UpdateConversation(Conversation conversation);
        Task DeleteConversation(int conversationId);
    }
}
