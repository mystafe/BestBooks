using Microsoft.VisualBasic.Devices;
using System.Net;

namespace BestBooks
{
    public partial class Form1 : Form
    {
        List<Book> books = new List<Book>();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            WebClient client = new WebClient();

            string reply1 = String.Empty;
            string reply2 = String.Empty;
            try
            {
                reply1 = client.DownloadString("https://www.goodreads.com/list/show/18834.BBC_Top_200_Books");
                reply2 = client.DownloadString("https://www.goodreads.com/list/show/18834.BBC_Top_200_Books?page=2");

            }
            catch (Exception)
            {

                MessageBox.Show("internet bağlantınızı kontrol ediniz!");
            }

            string searchWord = "js-dataTooltip";
            int start1 = reply1.IndexOf(searchWord);
            int start2 = reply2.IndexOf(searchWord);
            reply1 = reply1.Substring(start1 + 60);
            reply2 = reply2.Substring(start2 + 60);
            var list1 = reply1.Split("<td width=\"130px\"").ToList();
            //            list1.RemoveRange(0, 1);
            list1.RemoveRange(list1.Count-1, 1);
            var list2 = reply2.Split("<td width=\"130px\"").ToList();

            list2.RemoveRange(list2.Count-1, 1);
            list1.AddRange(list2);
            list1.ForEach(item =>
            {
                int start = item.IndexOf("role='heading' aria-level='4'>");
                var item1 = item.Substring(start + 30);
                int end = item1.IndexOf("</span>");
                string item2 = item1.Substring(0, (end));
                Book book = new Book(item2);
                books.Add(book);

            });

            lblResult.Text = SetLabel(books);
            dataGridView1.DataSource = books;



        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string input = textBoxSearch.Text.ToLower();
            List<Book> searchedBooks = new List<Book>();

            foreach (var item in books)
            {
                if (item.Name.ToLower().Contains(input))
                {
                    searchedBooks.Add(item);
                }
            }
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = searchedBooks;
            lblResult.Text = SetLabel(searchedBooks);
        }


        public static string SetLabel(List<Book> books)
        {

            return books.Count.ToString() + " adet kitap var.";
        }


    }
}