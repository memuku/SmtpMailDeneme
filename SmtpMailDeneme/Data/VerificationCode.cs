namespace SmtpMailDeneme.Data
{
    public class VerificationCode
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("UserEmail")]
        public string UserEmail { get; set; }
        [BsonElement("Code")]
        public string Code { get; set; }
        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }
    }
}
