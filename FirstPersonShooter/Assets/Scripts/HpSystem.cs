using UnityEngine;
using UnityEngine.UI;

public class HpSystem : MonoBehaviour
{
    public Image hpImage;
    public Image hpImage1;
    public float maxHP = 100;
    private float currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }
    private void Update()
    {
        if (hpImage != null) 
            hpImage.fillAmount = currentHP/maxHP;
        if (hpImage1 != null)
            hpImage1.fillAmount = currentHP / maxHP;

        if (currentHP <= 0) Destroy(gameObject) ;
    }

    public static void TakeDamage(GameObject target, float damage)
    {
        target.GetComponent<HpSystem>().currentHP -= damage;
    }
}
