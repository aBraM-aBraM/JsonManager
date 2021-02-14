using System;
using System.Collections.Generic;
using System.Text;

namespace JsonDB
{
    class Menu
    {

        public Menu() => Loop();
        public void Loop()
        {
            int loop_val = 0;
            do
            {
                if (Console.KeyAvailable)
                {
                    loop_val = HandleInput(Console.ReadKey().Key);
                }
            } while (loop_val != 1);
        }

        public virtual int HandleInput(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape) return 1;
            return 0;
        }

        protected virtual void Print() { }
    }
}
