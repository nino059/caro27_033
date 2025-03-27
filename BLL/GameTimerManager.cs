﻿using caro27_033.Class;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace caro27_033.BLL
{
    public class GameTimerManager
    {
        #region Properties

        private System.Windows.Forms.Timer gameTimer = new();
        private ProgressBar progressBar1;
        private ProgressBar progressBar2;
        private int currentPlayer;
        private Action<int> onTimeOut; // Callback khi hết thời gian

        #endregion

        #region Initialize
        public GameTimerManager(ProgressBar prgBar1, ProgressBar prgBar2, Action<int> timeOutCallback)
        {
            this.progressBar1 = prgBar1;
            this.progressBar2 = prgBar2;
            this.onTimeOut = timeOutCallback;
            InitializeTimer();
            SetupProgressBars();
        }

        /// <summary>
        /// Khởi tạo timer đếm ngược
        /// </summary>
        private void InitializeTimer()
        {
            gameTimer = new System.Windows.Forms.Timer
            {
                Interval = 100 // Cập nhật mỗi 100ms để có hiệu ứng mượt mà
            };
            gameTimer.Tick += GameTimer_Tick;
        }

        /// <summary>
        /// Thiết lập các ProgressBar
        /// </summary>
        private void SetupProgressBars()
        {
            // Thiết lập ProgressBar cho người chơi 1
            if (progressBar1 != null)
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = Cons.TIME_LIMIT * 10; // *10 vì interval là 100ms
                progressBar1.Value = progressBar1.Maximum;
                progressBar1.Step = 1;
            }

            // Thiết lập ProgressBar cho người chơi 2
            if (progressBar2 != null)
            {
                progressBar2.Minimum = 0;
                progressBar2.Maximum = Cons.TIME_LIMIT * 10;
                progressBar2.Value = progressBar2.Maximum;
                progressBar2.Step = 1;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Xử lý sự kiện Tick của Timer
        /// </summary>
        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            // Lấy ProgressBar của người chơi hiện tại
            ProgressBar currentProgressBar = currentPlayer == 0 ? progressBar1 : progressBar2;

            // Giảm giá trị ProgressBar
            if (currentProgressBar != null && currentProgressBar.Value > 0)
            {
                currentProgressBar.Value--;
            }

            // Khi hết thời gian
            if (currentProgressBar != null && currentProgressBar.Value == 0)
            {
                StopTimer();
                onTimeOut?.Invoke(currentPlayer);
            }
        }

        /// <summary>
        /// Bắt đầu đếm ngược
        /// </summary>
        public void StartCountdown(int player)
        {
            // Dừng timer hiện tại nếu đang chạy
            StopTimer();

            // Cập nhật người chơi hiện tại
            currentPlayer = player;

            // Xác định ProgressBar hiện tại và ProgressBar của đối thủ
            ProgressBar currentProgressBar = currentPlayer == 0 ? progressBar1 : progressBar2;
            ProgressBar otherProgressBar = currentPlayer == 0 ? progressBar2 : progressBar1;

            // Hiện ProgressBar hiện tại
            if (currentProgressBar != null)
            {
                currentProgressBar.Visible = true;
            }

            // Ẩn và reset ProgressBar của đối thủ
            if (otherProgressBar != null)
            {
                otherProgressBar.Visible = false;
                otherProgressBar.Value = otherProgressBar.Maximum;
            }

            // Bắt đầu đếm ngược
            gameTimer.Start();
        }

        /// <summary>
        /// Dừng timer
        /// </summary>
        public void StopTimer()
        {
            if (gameTimer != null && gameTimer.Enabled)
            {
                gameTimer.Stop();
            }
        }

        public void ResumeTimer()
        {
            gameTimer.Start();
        }

        /// <summary>
        /// Giải phóng tài nguyên
        /// </summary>
        public void Dispose()
        {
            if (gameTimer != null)
            {
                gameTimer.Stop();
                gameTimer.Tick -= GameTimer_Tick; // Quan trọng: hủy đăng ký sự kiện
                gameTimer.Dispose();
            }
        }
        #endregion
    }
}
