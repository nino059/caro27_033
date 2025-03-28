using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace caro27_033.Entity
{
    public class Player
    {
        public string Name { get; set; }
        public Image Mark { get; set; }
        public int Wins { get; set; } // Thêm thuộc tính lưu số trận thắng

        public Player(string name, Image mark)
        {
            Name = name;
            Mark = mark;
            Wins = 0; // Mặc định ban đầu chưa thắng trận nào
        }
    }
}