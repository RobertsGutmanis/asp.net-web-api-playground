namespace API.DTOs
{
    public class LoginDTO
    {
        public string Email {get; set;}
        public string Password {get; set;}

        public LoginDTO(){
            if(Email == null){
                Email = "";
            }
            if(Password == null){
                Password = "";
            }
        }
    }
}