using NLog;
using System.Linq;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

string choice;
var db = new BloggingContext();

do{
    Console.WriteLine("Blog Assignment");
    Console.WriteLine("1. Display all blogs");
    Console.WriteLine("2. Add blog");
    Console.WriteLine("3. Create Post");
    Console.WriteLine("4. Display post");
    Console.WriteLine("Choose an option or enter any other key to exit");
    choice = Console.ReadLine();

        if(choice == "1" ){
            // Display all Blogs from the database
            var query = db.Blogs.OrderBy(b => b.Name);

            Console.WriteLine("All blogs in the database:");
            foreach (var item in query)
            {
                Console.WriteLine(item.Name);
            }
        }
        if(choice == "2"){
            Console.WriteLine("Would you like to add a blog? (Y/N)");
            string resp = Console.ReadLine().ToUpper();

            if (resp != "Y") {break;}
            
            Console.WriteLine("Enter blog name: ");
            var name = Console.ReadLine();

            var blog = new Blog {Name = name};

            // var db = new BloggingContext();
            db.AddBlog(blog);
            logger.Info("Blog added: {name}", name);

        }
        if(choice == "3"){
            Console.WriteLine("Would you like to create a post? (Y/N)");
            string resp = Console.ReadLine().ToUpper();

            if (resp != "Y") {break;}

            Console.WriteLine("Enter name of blog you'd like to post to: ");
            string blogName = Console.ReadLine();

            using (var context = new BloggingContext())
            {
                var blog = context.Blogs
                            .Where(b => b.Name == blogName)
                            .FirstOrDefault();
                var key = context.Blogs
                            .Where(b => b.Name == blogName)
                            .FirstOrDefault()
                            .BlogId;
                

                if (blog == null)
                {
                    Console.WriteLine("There is no blog with that name.");
                }
                if(blog != null) 
                {
                    Console.WriteLine(key);
                    Console.WriteLine("Enter post title: ");
                    var title = Console.ReadLine();


                    Console.WriteLine("Post Content: ");
                    var content = Console.ReadLine();
                    

                    var post = new Post {Title = title, Content = content, BlogId = key};

                    db.Add(new Post {Title = title, Content = content, BlogId = key});
                
                    db.SaveChanges();
                  
                }
            }
            
        }
        if(choice == "4"){

            Console.WriteLine("Would you like to view posts?");
            string resp = Console.ReadLine().ToUpper();
            
            if(resp != "Y") {break;}

            Console.WriteLine("Which blog's post would you like to view?");
            string blogName = Console.ReadLine();

            using (var context = new BloggingContext())
            {
                var blog = context.Blogs
                            .Where(b => b.Name == blogName)
                            .FirstOrDefault();

                if (blog == null)
                {
                    Console.WriteLine("Blog does not exist.");
                }
                if(blog != null)
                {
                     var query = db.Blogs.OrderBy(b => b.Name);

                     Console.WriteLine("All posts in {name}: ", blogName);

                     foreach(var post in query)
                     {
                        Console.WriteLine(post);
                     }
                }
            }


        }

}while (choice == "1" || choice == "2" || choice == "3" || choice == "4");
logger.Info("Program ended");
