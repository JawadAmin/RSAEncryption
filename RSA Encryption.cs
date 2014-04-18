using System;
using System.Linq;
using System.Text;
using System.IO;

class RSAEncrypt
{
        //Acknowledgement: Obtained from online source
        public static ulong RepeatedSquaring(ulong a, ulong b, ulong n)
        {
            if (b == 0)
            {
                return 1;
            }
            ulong t = RepeatedSquaring(a, b / 2, n);
            
            ulong c = (t * t) % n;
            
            if (b % 2 == 1)
            {
                c = (c * a) % n;
            }
            
            return  c;
        }
        
        public static string[] convertToChars(string plainText)
        {
            char[] charArray = plainText.ToCharArray();
            string[] splitChars = new string[charArray.Length];

            for (int i = 0; i < splitChars.Length; i++)
            {
                splitChars [i] = charArray[i].ToString();
            }

            string [] charSets = new string [splitChars.Length/4];

            ulong j = 0;
            for (int i = 0; i < charSets.Length; i++)
            {
                for (ulong c = j; c < j + 4; c++)
                {
                    charSets[i] = charSets[i] + splitChars[c];
                }

                j = j + 4;
            }
            
            return charSets; 
        }
        
        public static void writeToStream(ulong[] cipherSetsP, ulong n, ulong e)
        {
            using (StreamWriter txtFile = new StreamWriter("Output.txt"))
            {
                txtFile.Write("{0} {1} {2} ", n, e, cipherSetsP.Length);
                for (ulong v = 0; v < (ulong)cipherSetsP.Length; v++)
                {
                    if (v != (ulong)(cipherSetsP.Length - 1))
                    {
                        txtFile.Write("{0} ", cipherSetsP[v]);
                    }

                    else
                    {
                        txtFile.Write("{0}", cipherSetsP[v]);
                    }   
                }
            }
        }
    
        public static ulong[] convertToASCII(string[] charSets)
        {
            ulong[] charsASCII = new ulong[charSets.Length];

            for (int i = 0; i < charsASCII.Length; i++)
            {
                byte[] value = Encoding.ASCII.GetBytes(charSets[i]);
            
                ulong sum = (ulong)(value[3] + value[2]*256 + value[1]*256*256 + value[0]*256*256*256);
          
                charsASCII[i] = sum;
            }
        
            return charsASCII;
        }

        public static ulong[] turnToCipher(ulong[] array, ulong d, ulong n)
        {
            ulong[] newArray = new ulong[array.Length];
            
            for (int i = 0; i < newArray.Length; i++)
            { 
                newArray[i] = RepeatedSquaring(array[i], d, n);
            }
            
            return newArray;
        }
        
        
        static void Main()
        {
            ulong p = 50021;
            ulong q = 50023;
            ulong phi = (p-1)*(q-1);
            ulong n = p * q;
            ulong d = 266719199;
            ulong e = 1048799;
            ulong nP = 3125033603;
            ulong eP = 52741219;

            string plainText = "Hello D.E.A.R. this is my little secret: 50021 and 50023. I hope you are having a great day and are healthy and happy.";
        
            writeToStream(turnToCipher(turnToCipher(convertToASCII(convertToChars(plainText)), d, n), eP, nP), n, e);
        }
}