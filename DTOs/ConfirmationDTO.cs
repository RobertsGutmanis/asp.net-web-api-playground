namespace API.DTOs
{
    public class ConfirmationDTO
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ConfirmationDTO(){
            if(PasswordHash == null){
                PasswordHash = new byte[0];
            }
            if(PasswordSalt == null){
                PasswordSalt = new byte[0];
            }
        }
    }
}