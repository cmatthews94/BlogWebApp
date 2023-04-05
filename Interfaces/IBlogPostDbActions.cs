using BlogWebApp.Models;

namespace BlogWebApp.Interfaces
{
    public interface IBlogPostDbActions
    {
        void AddNewBlogPostDataToDb(BlogPost blogPostData);
        BlogPost GetBlogPostFromDbById(int id);
        void UpdateBlogPostContent(int id, string NewBlogPostContent);
        void DeleteBlogPostById(int id);


    }
}
