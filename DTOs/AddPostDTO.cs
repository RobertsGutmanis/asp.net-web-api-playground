namespace API.DTOs
{
    public class AddPostDTO
    {
        public string PostTitle { get; set; }
        public string PostContent { get; set; }

        public AddPostDTO(){
            if(PostTitle == null){
                PostTitle = "";
            }
            if(PostContent == null){
                PostContent = "";
            } 
        }
    }
}