using UnityEngine;

namespace Data
{
    [System.Serializable]
	/// <summary>
	/// 아이템의 기본적인 정보를 가지고 있는 클래스
	/// </summary>
	public class ItemInfo
    {
        public string Name;
        public ItemID Id;
		public string Class;
		public float Hp;
        public float Atk;
        public float Ats;
        public float Afs;
        public int Weight;

        public static float WeightToSpeed(int weight)
        {
           var speed = (Mathf.Pow(weight, 2) + 20) * 0.01f;
           return speed;
        }
	
        public static ItemInfo operator+(ItemInfo origin, ItemInfo other)
        {
            origin.Hp += other.Hp;
            origin.Atk += other.Atk;
            origin.Ats -= other.Ats;
            origin.Afs -= other.Afs;
            origin.Weight += other.Weight;
            return origin;
        }
	}
}