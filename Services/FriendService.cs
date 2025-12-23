using Microsoft.EntityFrameworkCore;
using projec.Data;
using projec.Models;

namespace projec.Services
{
    public class FriendService : IFriendService
    {
        private readonly AppDbContext _context;

        public FriendService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> SendFriendRequest(int requesterId, string receiverUsername)
        {
            var receiver = await _context.Users.FirstOrDefaultAsync(u => u.Username == receiverUsername);
            if (receiver == null) return "Kullanıcı bulunamadı.";
            if (receiver.Id == requesterId) return "Kendine istek atamazsın.";

            // Zaten istek var mı veya arkadaşlar mı?
            var existing = await _context.Friendships.FirstOrDefaultAsync(f =>
                (f.RequesterId == requesterId && f.ReceiverId == receiver.Id) ||
                (f.RequesterId == receiver.Id && f.ReceiverId == requesterId));

            if (existing != null)
            {
                if (existing.Status == FriendshipStatus.Accepted) return "Zaten arkadaşsınız.";
                if (existing.Status == FriendshipStatus.Pending) return "Zaten bekleyen bir istek var.";
                // Eğer reddedildiyse tekrar istek atılabilir mi? Şuanlık evet diyelim veya hayır.
                // Basit olsun diye tekrar atılamaz diyelim, ya da kaydı sildirip yeni açtırabiliriz.
                return "Daha önce bir etkileşiminiz olmuş."; 
            }

            var friendship = new Friendship
            {
                RequesterId = requesterId,
                ReceiverId = receiver.Id,
                Status = FriendshipStatus.Pending
            };

            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();
            return "İstek gönderildi.";
        }

        public async Task<string> AcceptFriendRequest(int requestId, int userId)
        {
            var request = await _context.Friendships.FirstOrDefaultAsync(f => f.Id == requestId && f.ReceiverId == userId);
            
            if (request == null) return "İstek bulunamadı.";
            if (request.Status != FriendshipStatus.Pending) return "Bu istek zaten sonuçlanmış.";

            request.Status = FriendshipStatus.Accepted;
            await _context.SaveChangesAsync();
            return "Arkadaşlık kabul edildi!";
        }

        public async Task<string> RejectFriendRequest(int requestId, int userId)
        {
            var request = await _context.Friendships.FirstOrDefaultAsync(f => f.Id == requestId && f.ReceiverId == userId);

            if (request == null) return "İstek bulunamadı.";

            request.Status = FriendshipStatus.Rejected; 
            // Veya direkt silebiliriz: _context.Friendships.Remove(request);
            // Öğrenci projesi olduğu için Rejected olarak işaretleyip bırakalım, veritabanında dursun.
            
            await _context.SaveChangesAsync();
            return "İstek reddedildi.";
        }

        public async Task<List<User>> GetFriends(int userId)
        {
            // Arkadaşlıklar iki yönlü olabilir (Ben ekledim veya O ekledi) ve ACCEPTED olmalı.
            var friendships = await _context.Friendships
                .Include(f => f.Requester)
                .Include(f => f.Receiver)
                .Where(f => (f.RequesterId == userId || f.ReceiverId == userId) && f.Status == FriendshipStatus.Accepted)
                .ToListAsync();

            var friends = new List<User>();
            foreach (var f in friendships)
            {
                if (f.RequesterId == userId) friends.Add(f.Receiver);
                else friends.Add(f.Requester);
            }

            return friends;
        }

        public async Task<List<Friendship>> GetPendingRequests(int userId)
        {
            // Bana gelen ve bekleyen istekler
            return await _context.Friendships
                .Include(f => f.Requester)
                .Where(f => f.ReceiverId == userId && f.Status == FriendshipStatus.Pending)
                .ToListAsync();
        }
    }
}
