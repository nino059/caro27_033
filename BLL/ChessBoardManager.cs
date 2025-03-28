using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using caro27_033.Entity;
using caro27_033.GUI;
using caro27_033.Class;

namespace caro27_033.BLL
{
    public class ChessBoardManager
    {
        #region Properties
        private Panel chessBoard;
        private PictureBox avartar1;
        private PictureBox avartar2;
        private Label labelName1;
        private Label labelName2;
        private ProgressBar progressBar1;
        private ProgressBar progressBar2;

        // Quản lý timer riêng biệt
        private GameTimerManager timerManager = null!;// null là đảm bảo có giá trị khởi tạo, null! là trì hoãn khởi tạo cho đến constructor của ChessBoardManager

        private string imagePathX = $"{Application.StartupPath}\\Resources\\X.png"; // Đường dẫn ảnh X
        private string imagePathO = $"{Application.StartupPath}\\Resources\\O.png"; // Đường dẫn ảnh O

        private int currentPlayer = 0;

        private CheckWin checkWin;

        private Image imageX;
        private Image imageO;


        public Panel ChessBoard
        {
            get { return chessBoard; }
            set { chessBoard = value; }
        }
        public PictureBox Avatar1
        {
            get { return avartar1; }
            set { avartar1 = value; }
        }
        public PictureBox Avatar2
        {
            get { return avartar2; }
            set { avartar2 = value; }
        }
        public Label LabelName1
        {
            get { return labelName1; }
            set { labelName1 = value; }
        }
        public Label LabelName2
        {
            get { return labelName2; }
            set { labelName2 = value; }
        }
        public ProgressBar ProgressBar1
        {
            get { return progressBar1; }
            set { progressBar1 = value; }
        }
        public ProgressBar ProgressBar2
        {
            get { return progressBar2; }
            set { progressBar2 = value; }
        }

        public List<List<Button>> matrix = new List<List<Button>>();
        public List<Player> Player { get; set; } = new List<Player>();
        public GameTimerManager TimerManager { get => timerManager; set => timerManager = value; }

        #endregion

        #region Initialize
        public ChessBoardManager(Panel chessBoard, PictureBox avt1, PictureBox avt2, Label lab1, Label lab2, ProgressBar prgBar1, ProgressBar prgBar2)
        {
            this.chessBoard = chessBoard;
            this.avartar1 = avt1;
            this.avartar2 = avt2;
            this.labelName1 = lab1;
            this.labelName2 = lab2;
            this.progressBar1 = prgBar1;
            this.progressBar2 = prgBar2;
            checkWin = new CheckWin(matrix);

            imageX = File.Exists(imagePathX) ? Image.FromFile(imagePathX) : new Bitmap(1, 1);
            imageO = File.Exists(imagePathO) ? Image.FromFile(imagePathO) : new Bitmap(1, 1);

            // Khởi tạo người chơi
            Player.Add(new Player("Player 1", imageX));
            Player.Add(new Player("Player 2", imageO));
                        
            // Khởi tạo timer manager
            TimerManager = new GameTimerManager(progressBar1, progressBar2, OnTimeOut);

            // Khời tạo bàn cờ
            DrawChessBoard();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Tạo bàn cờ lần đầu
        /// </summary>
        public void DrawChessBoard()
        {

            // Xóa bàn cờ cũ
            chessBoard.Controls.Clear();

            // Khởi tạo ma trận button
            matrix.Clear();

            // Tạo danh sách để lưu tất cả các button
            List<Button> allButtons = new List<Button>();

            for (int i = 0; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                matrix.Add(new List<Button>());

                for (int j = 0; j < Cons.CHESS_BOARD_WIDTH; j++)
                {
                    Button btn = new Button()
                    {
                        Width = Cons.CHESS_WIDTH,
                        Height = Cons.CHESS_HEIGHT,
                        Location = new Point(j * Cons.CHESS_WIDTH, i * Cons.CHESS_HEIGHT),
                        BackgroundImageLayout = ImageLayout.Stretch, // Căn chỉnh ảnh vừa với button
                        Tag = i.ToString(), // Lưu vị trí dòng để dễ xử lý sau này
                        TabStop = false
                    };

                    btn.Click += Btn_Click; // Thêm sự kiện click
                    matrix[i].Add(btn); // Thêm button vào ma trận
                    allButtons.Add(btn); // Thêm vào danh sách tạm
                }
            }

            // Thêm tất cả các button vào panel cùng một lúc
            chessBoard.Controls.AddRange(allButtons.ToArray());

            // Reset màu nền của tên người chơi
            labelName1.BackColor = Color.White;
            labelName2.BackColor = Color.White;

            // Thiết lập người chơi đầu tiên
            currentPlayer = 0;

            // Chuyển lượt
            ChangePlayer();

            // Bắt đầu đếm ngược cho người chơi đầu tiên
            TimerManager.StartCountdown(currentPlayer);
        }

        /// <summary>
        /// Click chọn nước đi, kiểm tra nước đi hợp lệ? Kiểm tra đã win hay chưa?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void Btn_Click(object? sender, EventArgs e)
        {
            Button? btn = sender as Button;

            // Nếu button đã có ảnh thì không xử lý, nếu vị trí đã có quân thì không được đánh
            if (btn == null || btn.BackgroundImage != null)
                return;

            // Hiển thị ảnh X hoặc O tùy thuộc vào người chơi hiện tại
            btn.BackgroundImage = Player[currentPlayer].Mark;


            // Kiểm tra điều kiện chiến thắng
            checkWin = new CheckWin(matrix);
            int row = Convert.ToInt32(btn.Tag);
            int col = matrix[row].IndexOf(btn);

            if (checkWin.Check(row, col))
            {
                TimerManager.StopTimer();
                DialogResult result = MessageBox.Show("Bạn muốn chơi tiếp?", $"{Player[currentPlayer].Name} đã thắng!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                    ResetChessBoard();
                else
                    Application.Exit();

                return;
            }


            // Dừng timer hiện tại          
            TimerManager.StopTimer();

            // Chuyển lượt 
            currentPlayer = currentPlayer == 0 ? 1 : 0;
            ChangePlayer();
            TimerManager.StartCountdown(currentPlayer);
        }

        /// <summary>
        /// Xử lý khi hết thời gian
        /// </summary>
        private void OnTimeOut(int player)
        {
            TimerManager.StopTimer();
            MessageBox.Show($"{Player[currentPlayer].Name} đã thua do hết thời gian!", "Hết thời gian", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ResetChessBoard();
        }

        /// <summary>
        /// Reset bàn cờ mà không tạo lại các button
        /// </summary>
        public void ResetChessBoard()
        {
            // Xóa hình ảnh nền của tất cả các button, cụ thể là xóa hết các vị trí mà button đã đánh dấu
            foreach (List<Button> buttonRow in matrix)
            {
                foreach (Button button in buttonRow)
                {
                    button.BackgroundImage = null;
                }
            }

            // Reset màu nền của tên người chơi
            labelName1.BackColor = Color.White;
            labelName2.BackColor = Color.White;

            // Chuyển lượt người chơi
            currentPlayer = currentPlayer == 0 ? 1 : 0;
            ChangePlayer();
            TimerManager.StartCountdown(currentPlayer);

        }

        /// <summary>
        /// Đổi lượt hiển thị label
        /// </summary>
        private void ChangePlayer()
        {
            // Reset màu nền
            labelName1.BackColor = Color.White;
            labelName2.BackColor = Color.White;

            // Thay đổi hiển thị tên người chơi hiện tại
            if (currentPlayer == 0)
                labelName1.BackColor = Color.Green;
            else
                labelName2.BackColor = Color.Green;
        }


        #endregion
    }
}
