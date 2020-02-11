using UnityEngine;

namespace Ibit.CakeGame
{
    public class Candles : MonoBehaviour
    {
        public Sprite candleOff;
        public Sprite candleOn;
        public Sprite[] candlesOn = new Sprite[5];
        public Animator[] candlesAnim;

        public void TurnOff(int index)
        {
            candlesAnim[index].SetBool("TurnOff", true);
        }

        public void TurnOn()
        {
            for (int i= 0; i<=21; i++)                      //add 21/10/19
            {
                candlesAnim[i].SetBool("TurnOff", false);           //add 21/10/19
            }

           // candlesAnim[0].SetBool("TurnOff", false);            //código de x/09/19   ___OBS:_____tava funcionando 100%, caso alteração acima der problema, restaurar__________(deletavel em revisões futuras)_____ 
           // candlesAnim[1].SetBool("TurnOff", false);
           // candlesAnim[2].SetBool("TurnOff", false);
           // candlesAnim[3].SetBool("TurnOff", false);
           // candlesAnim[4].SetBool("TurnOff", false);
           // candlesAnim[5].SetBool("TurnOff", false);
          //  candlesAnim[6].SetBool("TurnOff", false);
           // candlesAnim[7].SetBool("TurnOff", false);
          //  candlesAnim[8].SetBool("TurnOff", false);
          //  candlesAnim[9].SetBool("TurnOff", false);
          //  candlesAnim[10].SetBool("TurnOff", false);
           // candlesAnim[11].SetBool("TurnOff", false);
          //  candlesAnim[12].SetBool("TurnOff", false);
          //  candlesAnim[13].SetBool("TurnOff", false);
          //  candlesAnim[14].SetBool("TurnOff", false);
           // candlesAnim[15].SetBool("TurnOff", false);
          //  candlesAnim[16].SetBool("TurnOff", false);
          //  candlesAnim[17].SetBool("TurnOff", false);
          //  candlesAnim[18].SetBool("TurnOff", false);
          //  candlesAnim[19].SetBool("TurnOff", false);
          //  candlesAnim[20].SetBool("TurnOff", false);
          //  candlesAnim[21].SetBool("TurnOff", false);




            //gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = candleOn;   //código original
            //gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = candleOn;
            //gameObject.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = candleOn;
        }
    }
}