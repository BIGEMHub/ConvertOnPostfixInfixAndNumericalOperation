using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertOnPostfixInfixAndNumericalOperation
{
    public class Block
    {
        public string data;
        public Block next;
        public Block(string Data, Block Last)
        {
            this.data = Data;
            this.next = Last;
        }
    } // Infix den PostFix e çevirme için oluşturuldu
    public class BlockMat
    {
        public double operand;
        public BlockMat next;
        public BlockMat(double operand, BlockMat Veri)
        {
            this.operand = operand;
            this.next = Veri;
        }
    } // Postfix üzerinde Matematiksel İşlem Yapılması için Oluşturuldu
    public class StackDosyam
    {
        public Block first;
        public int count;
        public StackDosyam()
        {
            this.first = null;
            this.count = 0;
        }
        public int Kapasite()
        {
            return this.count;
        }
        public bool BoşDoluKont()
        {
            if (this.Kapasite() > 0) return false;
            else return true;
        }
        public void Ekle(string Veri)
        {
            this.first = new Block(Veri, this.first);
            this.count++;
        }
        public string Cikart()
        {
            var temp = "";
            if (this.BoşDoluKont() == false)
            {
                temp = this.first.data;
                this.first = this.first.next;
                this.count--;
            }
            return temp;
        }
        public string Göster()
        {
            if (this.BoşDoluKont() == false) return this.first.data; //if(!this.BoşDoluKont()) şeklinde de kullanılabilir
            else return "";
        }
    } // Dönüşüm için kararkterleri tutan stack yapısı
    public class MatStack
    {

        public BlockMat Last1;
        public int count;
        public MatStack()
        {
            this.Last1 = null;
            this.count = 0;
        }
        public int Kapasite()
        {
            return this.count;
        }
        public bool BoşDoluKont()
        {
            if (this.Kapasite() > 0) return false;
            else return true;
        }

        public void Push(double value)
        {
            this.Last1 = new BlockMat(value, this.Last1);
            this.count++;
        }

        public int Pop()
        {
            var value = this.Peek();
            if (this.BoşDoluKont() == false)
            {
                this.Last1 = this.Last1.next;
                this.count--;
            }
            return value;
        }

        public int Peek()
        {
            if (!this.BoşDoluKont()) return (int)this.Last1.operand;
            else return 0;
        }
    }// Matematiksel İşlem için kararkterleri tutan stack yapısı

    #region ConverToInfix Postfix İfadenin Infix İfadeye Çevrildiği sınıf
    public class Donüsüm
    {
        public bool Oprtr(char Text)
        {
            if (Text == '+' || Text == '-' ||
                Text == '*' || Text == '/' ||
                Text == '^' || Text == '%')
                return true;
            return false;
        }
        public bool Oprnt(char Text)
        {
            if ((Text >= '0' && Text <= '9') ||
                (Text >= 'a' && Text <= 'z') ||
                (Text >= 'A' && Text <= 'Z'))
                return true;
            return false;
        }
        public void PostfixToInfix(string Postfix)
        {
            var Kapasite = Postfix.Length;
            var MyStack = new StackDosyam();

            var Ek = "";
            var op1 = "";
            var op2 = "";
            var Kontrol = true;
            for (var i = 0; i < Kapasite && Kontrol; i++)
            {
                if (Oprtr(Postfix[i]))
                {
                    if (MyStack.Kapasite() > 1)
                    {
                        op1 = MyStack.Cikart();
                        op2 = MyStack.Cikart();
                        Ek = "(" + op2 + Postfix[i] + op1 + ")";
                        MyStack.Ekle(Ek);
                    }
                    else Kontrol = false;
                }
                else if (Oprnt(Postfix[i]))
                {
                    Ek = Postfix[i].ToString();
                    MyStack.Ekle(Ek);
                }
                else Kontrol = false;
            }
            if (Kontrol == false) Console.WriteLine("Geçersiz postfix : " + Postfix);
            else
            {
                Console.WriteLine(" Postfix:\t" + Postfix);
                Console.WriteLine(" Infix  :\t" + MyStack.Cikart());
            }
        }
    }
    #endregion 
    #region Matematiksel Hesaplama
    public class Matematik // Girilen Rakamlar Üzerinden İşlem Yapılabilmektedir.
                           // İki ve üstü basamaklı sayılarda yanlış sonuç verecektir.
    {
        public void MatHesapla(string Postfix)
        {
            var size = Postfix.Length;
            int a = 0;
            int b = 0;

            var s = new MatStack();
            var DeğerKont = true;

            for (var i = 0; i < size && DeğerKont; i++)
            {
                if (Postfix[i] >= '0' && (int)Postfix[i] <= '9')
                {
                    a = (int)(Postfix[i]) - (int)('0');
                    s.Push(a);
                }
                else if (s.Kapasite() > 1)
                {
                    a = s.Pop();
                    b = s.Pop();

                    if (Postfix[i] == '+') s.Push(b + a);

                    else if (Postfix[i] == '-') s.Push(b - a);

                    else if (Postfix[i] == '*') s.Push(b * a);

                    else if (Postfix[i] == '/') s.Push((double)(b / a));

                    else if (Postfix[i] == '%') s.Push(b % a);


                    else DeğerKont = false;

                }
                else if (s.Kapasite() == 1)
                {
                    if (Postfix[i] == '-')
                    {
                        a = s.Pop();
                        s.Push(-a);
                    }
                    else if (Postfix[i] != '+') DeğerKont = false;
                }
                else DeğerKont = false;

            }
            if (DeğerKont == false)
            {
                Console.WriteLine(Postfix + " Yanlış Denklem İçeriği Tespit Edildi ");
                return;
            }
            Console.WriteLine(Postfix + " = " + s.Pop());
        }
    }
    #endregion

    #region Infix İfadenin Postfix e Çevrilmesi
    public class ConvertToPostfix
    {
        public string ConvertPostFix(string InFix)
        {
            string PostFix = "";
            Stack<char> Oprtr = new Stack<char>();
            for (int i = 0; i < InFix.Length; i++)
            {
                char Karakter = InFix[i];
                if (char.IsDigit(Karakter))
                {
                    while (i < InFix.Length && char.IsDigit(InFix[i]))
                    {
                        PostFix += InFix[i];
                        i++;
                    }
                    i--;
                    continue;
                }
                else if (Karakter == '(') Oprtr.Push(Karakter);
                else if (Karakter == '*' || Karakter == '+' || Karakter == '-' || Karakter == '/' || Karakter == '^')
                {
                    while (Oprtr.Count != 0 && Oprtr.Peek() != '(')
                    {
                        if (OnKontrol(Oprtr.Peek(), Karakter))
                            PostFix += Oprtr.Pop();
                        else
                            break;
                    }
                    Oprtr.Push(Karakter);
                }
                else if (Karakter == ')')
                {
                    while (Oprtr.Count != 0 && Oprtr.Peek() != '(')
                    {
                        PostFix += Oprtr.Pop();
                    }
                    if (Oprtr.Count != 0) Oprtr.Pop();
                }
                else PostFix += Karakter;
            }
            while (Oprtr.Count != 0)
            {
                PostFix += Oprtr.Pop();
            }
            return PostFix;
        }
        public bool OnKontrol(char IlkIslem, char IkinciIslem)
        {
            string Sıralama = "(+-*/^";
            int[] Oncelikler = { 0, 1, 1, 2, 2, 3 };
            int IlkIslemOnceligi = Sıralama.IndexOf(IlkIslem);
            int IkinciIslemOnceligi = Sıralama.IndexOf(IkinciIslem);
            return (Oncelikler[IlkIslemOnceligi] >= Oncelikler[IkinciIslemOnceligi]);
        }
    }
    #endregion
    class Program
    {
        public static void Main()
        {
            #region Postfix İfadenin Infix İfadeye Dönüşümü
            var Denk1 = new Donüsüm();
            var Postfix = "ab+c*ef+g/+";
            Denk1.PostfixToInfix(Postfix);
            #endregion
            #region Infix İfadenin Önce Postfix İfadeye Çevrilmesi Sonrasında Matematiksel(Sayısal) İşlem Yapılması
            Console.WriteLine("=====================\n" + "<<<<<<<<<<SAYISAL İŞLEM>>>>>>>>>>>>>");
            double a = 0;
            double b = 0;
            double c = 0;
            for (int k = 0; k < 3; k++)
            {
                Console.WriteLine("Diskriminantının Hesaplanmasını İstediğiniz Polinomun {0}. Katsayısını Giriniz(RAKAM):", k + 1);
                if (k == 0) a = Convert.ToDouble(Console.ReadLine());
                else if (k == 1) b = Convert.ToDouble(Console.ReadLine());
                else c = Convert.ToDouble(Console.ReadLine());
            }
            string Infix = string.Format(a + "x²" + "+" + b + "x" + "+" + c);
            Console.WriteLine("Diskriminantı Hesaplanacak Denklem:\t" + Infix);
            string DiskFormul = string.Format(b + "*" + b + "-" + 4 + "*" + a + "*" + c);
            var Denk2 = new ConvertToPostfix();
            var Postfix1 = Denk2.ConvertPostFix(DiskFormul);

            Console.WriteLine(Postfix1);
            var Denk3 = new Matematik();
            Denk3.MatHesapla(Postfix1);

            #endregion
            Console.ReadLine();
        }
    }
}
