using BlogWebApp.Interfaces;
using BlogWebApp.Models;

namespace BlogWebApp.Database
{
    public class BlogPostDbActions : IBlogPostDbActions
    {
        private readonly IUserAccountActions _userAccountActions;
        public BlogPostDbActions(IUserAccountActions userAccountActions)
        {
            _userAccountActions = userAccountActions;
        }

        public void AddNewBlogPostDataToDb(BlogPost blogPostData)
        {
            using (var _context = new BlogPostDbContext())
            {
                if (DoesPostTitleExist(blogPostData.Title) == true)
                {
                    throw new Exception("Post by that title already exists");

                }
                else
                {
                    _context.BlogPosts.Add(blogPostData);
                    _context.SaveChanges();
                }
            };
        }

        public BlogPost GetBlogPostFromDbById(int id)
        {
            using (var _context = new BlogPostDbContext())
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

        public bool DoesPostTitleExist(string PostName)
        {
            using (var _context = new BlogPostDbContext())
            {
                var s = from p in _context.BlogPosts where p.Title == PostName select p;
                if (s.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<BlogPost> GetAllBlogPostsForUsername(string username)
        {
            using (var _context = new BlogPostDbContext())
            {
                List<BlogPost> listOfPostsByUser = new List<BlogPost>();
                foreach (var blogPost in _context.BlogPosts)
                {
                    if (blogPost.Author == username)
                    {
                        listOfPostsByUser.Add(blogPost);
                    }
                }
                return listOfPostsByUser;
            }
        }
        public List<int> GetListOfNullPosts()
        {
            List<int> listOfNullPostIds = new List<int>();
            using (var _context = new BlogPostDbContext())
            {
                foreach (BlogPost post in _context.BlogPosts)
                {
                    if(post.Author == null)
                    {
                        listOfNullPostIds.Add(post.Id);
                    }
                    else if(post.Content == null)
                    {
                        listOfNullPostIds.Add(post.Id);
                    }
                }
                return listOfNullPostIds;
            }
        }

        public string GetAuthorFromPostId(int postId)
        {
            using(var _context = new BlogPostDbContext())
            {
                var foundBlogPost = _context.BlogPosts.FirstOrDefault(b => b.Id == postId);
                if (foundBlogPost == null)
                {
                    throw new Exception("Unable to find post with that Id");
                }
                else
                { 
                    return foundBlogPost.Author;
                }
            }
        }
    }
}
