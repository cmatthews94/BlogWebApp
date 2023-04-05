using BlogWebApp.Interfaces;
using BlogWebApp.Models;

namespace BlogWebApp.Database
{
    public class BlogPostDbActions : IBlogPostDbActions
    {
        public void AddNewBlogPostDataToDb(BlogPost blogPostData)
        {
            using (var _context = new BlogPostDbContext())
            {
                _context.BlogPosts.Add(blogPostData);
                _context.SaveChanges();
            };
        }

        public BlogPost GetBlogPostFromDbById(int id)
        {
            using(var _context = new BlogPostDbContext())
            {
                var foundBlogPost = _context.BlogPosts.FirstOrDefault(b => b.Id == id);
                if (foundBlogPost != null)
                {
                    return foundBlogPost;
                }
                else { throw new Exception("unable to find that Id"); }
            }
        }
        public void UpdateBlogPostContent(int id, string NewBlogPostContent) 
        {
            using (var _context = new BlogPostDbContext())
            {
                GetBlogPostFromDbById(id).Content = NewBlogPostContent;
                _context.SaveChanges();
            }
        }
        public void DeleteBlogPostById(int id)
        {
            using (var _context = new BlogPostDbContext())
            {
                _context.BlogPosts.Remove(GetBlogPostFromDbById(id));
                _context.SaveChanges();
            }
        }
    }
}
