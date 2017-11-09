using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{

    enum CardSuit { HEART, DIAMOND, CLUB, SPADE };
    enum CardName { Jack = 2, Queen, King, Six = 6, Seven, Eight, Nine, Ten, Ace };
    //enum CardOwner { Computer, Player };

    struct Card
    {
        public CardSuit Suit;
        public CardName Name;
        public int Volume;
    }

    class Program
    {


        static Card[] PackOfCard()
        {

            Card[] result = new Card[36];

            int cardNumber = 0;

            foreach (var suit in Enum.GetValues(typeof(CardSuit)))
            {

                foreach (var cName in Enum.GetValues(typeof(CardName)))
                {
                    Card Card = new Card();
                    Card.Suit = (CardSuit)suit;
                    Card.Name = (CardName)cName;
                    Card.Volume = (int)cName;

                    result[cardNumber++] = Card;
                }

            }
            return result;
        }

        static Card[] PackShuffle(Card[] pack)
        {
            Random rand = new Random();

            for (int i = 0; i < 1000; i++)
            {
                int cardNumber01 = rand.Next(0, 36);
                Card tempCard = new Card();
                tempCard = pack[cardNumber01];
                int cardNumber02 = rand.Next(0, 36);
                pack[cardNumber01] = pack[cardNumber02];
                pack[cardNumber02] = tempCard;
            }
            return pack;
        }

        static int ChooseFirstMove()
        {
            int firstMove = 0;
            bool inputRezult = false;
            while (inputRezult == false)
            {
                Console.Write("Who make first Move? (Computer - 1, Player - 2)");
                inputRezult = int.TryParse(Console.ReadLine(), out firstMove);
                if (inputRezult == true)
                {
                    if (firstMove < 1 || firstMove > 2)
                    {
                        inputRezult = false;
                    }
                }
                if (inputRezult == false)
                {
                    Console.WriteLine("Don't correct input.");
                }
            }
            return firstMove;
        }

        static void Main(string[] args)
        {
            Card[] packOfCards = PackOfCard();

            //foreach (var card in packOfCards)
            //{
            //    Console.WriteLine("{0} {1} - {2}", card.Name, card.Suit, card.Volume);
            //}

            Console.WriteLine("=============== After Shuffle ================");
            Card[] pack = PackShuffle(packOfCards);
            //foreach (var card in pack)
            //{
            //    Console.WriteLine("{0} {1} - {2}", card.Name, card.Suit, card.Volume);
            //}

            int whoFirstMove = ChooseFirstMove();
            int whoMove = whoFirstMove;

            Card[] playersCards = new Card[30];
            Card[] computersCards = new Card[30];

            bool finishMove = false;
            bool finishGame = false;

            int sumVolume = 0;
            int cardNumInPack = 0;
            int cardNumInPlayersPack = 0;
            int cardNumInComputersPack = 0;
            while (finishGame == false)
            {
                Card actCard = pack[cardNumInPack];
                if (whoMove == 1)
                {
                    if (cardNumInPack < 4)   // первоначальная раздача по 2 карты
                    {
                        computersCards[cardNumInComputersPack++] = actCard;
                        cardNumInPack++;
                    }
                    else
                    {
                        finishMove = false;
                        while (finishMove == false)
                        {
                            Console.WriteLine("===== Computers Cards");
                            sumVolume = 0;
                            foreach (var card in computersCards)
                            {
                                if (card.Volume == 0)
                                    break;
                                sumVolume = sumVolume + card.Volume;
                                Console.WriteLine("{0} {1} - {2}", card.Name, card.Suit, card.Volume);
                            }
                            Console.WriteLine("===== You have {0} points", sumVolume);
                            if (sumVolume == 21)
                            {
                                if (whoFirstMove == 1)
                                {
                                    Console.WriteLine("Yoy Lose. Comp WIN!!!");
                                    finishMove = true;
                                    finishGame = true;
                                    break;
                                }
                            }
                            else if (sumVolume > 21)
                            {
                                finishMove = true;
                                if (whoFirstMove == 2)
                                {
                                    finishGame = true;
                                }
                                break;
                            }
                            int result = 0;
                            bool inputRezult = false;
                            while (inputRezult == false)
                            {
                                Console.Write("Do You take next card (1) or finish (0)?");
                                inputRezult = int.TryParse(Console.ReadLine(), out result);
                                if (inputRezult == true)
                                {
                                    if (result < 0 || result > 1)
                                    {
                                        inputRezult = false;
                                    }
                                }
                                if (inputRezult == false)
                                {
                                    Console.WriteLine("Don't correct input. Choose only 0 or 1.");
                                }
                            }
                            if (result == 0)
                            {
                                finishMove = true;
                                if (whoFirstMove == 2)
                                {
                                    finishGame = true;
                                }
                            }
                            else
                            {
                                computersCards[cardNumInComputersPack++] = actCard;
                                actCard = pack[cardNumInPack++];
                            }
                        }
                    }
                    whoMove = 0;
                }
                else
                {
                    if (finishGame == true)
                    {
                        break;
                    }
                    if (cardNumInPack < 4)   // первоначальная раздача по 2 карты
                    {
                        playersCards[cardNumInPlayersPack++] = actCard;
                        cardNumInPack++;
                    }
                    else

                    {
                        finishMove = false;
                        while (finishMove == false)
                        {
                            Console.WriteLine("===== Players Cards");
                            sumVolume = 0;
                            foreach (var card in playersCards)
                            {
                                if (card.Volume == 0)
                                    break;
                                sumVolume = sumVolume + card.Volume;
                                Console.WriteLine("{0} {1} - {2}", card.Name, card.Suit, card.Volume);
                            }
                            Console.WriteLine("===== You have {0} points", sumVolume);
                            if (sumVolume == 21)
                            {
                                if (whoFirstMove == 1)
                                {
                                    Console.WriteLine("Yoy WIN!!! Comp is loser!!!");
                                    finishMove = true;
                                    finishGame = true;
                                    break;
                                }
                            }
                            else if (sumVolume > 21)
                            {
                                finishMove = true;
                                if (whoFirstMove == 1)
                                {
                                    finishGame = true;
                                }
                                break;
                            }
                            int result = 0;
                            bool inputRezult = false;
                            while (inputRezult == false)
                            {
                                Console.Write("Do You take next card (1) or finish (0)?");
                                inputRezult = int.TryParse(Console.ReadLine(), out result);
                                if (inputRezult == true)
                                {
                                    if (result < 0 || result > 1)
                                    {
                                        inputRezult = false;
                                    }
                                }
                                if (inputRezult == false)
                                {
                                    Console.WriteLine("Don't correct input. Choose only 0 or 1.");
                                }
                            }
                            if (result == 0)
                            {
                                finishMove = true;
                                if (whoFirstMove == 1)
                                {
                                    finishGame = true;
                                }
                            }
                            else
                            {
                                playersCards[cardNumInPlayersPack++] = actCard;
                                actCard = pack[cardNumInPack++];
                            }
                        }
                    }
                    whoMove = 1;
                };

            }
            Console.WriteLine("*****************************************************************");
            sumVolume = 0;
            Console.WriteLine("===== Players Cards:");
            foreach (var card in playersCards)
            {
                if (card.Volume == 0)
                    break;
                Console.WriteLine("{0} {1} - {2}", card.Name, card.Suit, card.Volume);
                sumVolume = sumVolume + card.Volume;
            }
            Console.WriteLine("===== Players total volume = {0}", sumVolume);
            Console.WriteLine("*****************************************************************");
            sumVolume = 0;
            Console.WriteLine("===== Computers Cards:");
            foreach (var card in computersCards)
            {
                if (card.Volume == 0)
                    break;
                Console.WriteLine("{0} {1} - {2}", card.Name, card.Suit, card.Volume);
                sumVolume = sumVolume + card.Volume;
            }
            Console.WriteLine("===== Computers total volume = {0}", sumVolume);
            Console.WriteLine("*****************************************************************");
            // comm
            Console.ReadKey();
        }
    }
}
