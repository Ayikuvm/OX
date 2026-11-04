using System.Collections;
using System.Windows.Forms;

namespace Snake
{
    internal class Input
    {
        // สร้างรายการปุ่มกด
        private static Hashtable keyTable = new Hashtable();

        // ฟังก์ชันเช็คว่าปุ่มถูกกดหรือไม่
        public static bool KeyPressed(Keys key)
        {
            if (keyTable[key] == null)
            {
                return false;
            }
            return (bool)keyTable[key];
        }

        // ฟังก์ชันบันทึกสถานะปุ่มกด
        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }
}