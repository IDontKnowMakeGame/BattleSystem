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
        public float CoolTime;

        public static float WeightToSpeed(int weight)
        {
           var speed = (Mathf.Pow(weight, 2) + 20) * 0.01f;
           return speed;
        }
        
        public float Speed => WeightToSpeed(Weight);
	
        public static ItemInfo operator+(ItemInfo origin, ItemInfo other)
        {
            ItemInfo item = new ItemInfo();
            item.Hp = origin.Hp + other.Hp;
            item.Atk = origin.Atk +  other.Atk;
            item.Ats = origin.Ats + other.Ats;
            item.Afs = origin.Afs + other.Afs;
            item.Weight = origin.Weight + other.Weight;
			item.CoolTime = origin.CoolTime + other.CoolTime;
            return origin;
        }
	}
}