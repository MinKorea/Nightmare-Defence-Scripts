using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerComponent : MonoBehaviour
{
    [SerializeField]
    int hp = 100;       // Current HP
    int maxHP = 100;    // Maximum HP
    int limitMaxHP = 200;      // �ִ� �þ �� �ִ� maxHP ��
    GameObject hpSlider;
    [SerializeField]
    GameObject dmgText;
    [SerializeField]
    float dmgTextYPos = 1.5f;

    bool isDead = false;

    Animator anit;
    [SerializeField]
    Material[] materials;

    SkinnedMeshRenderer smr;

    // Start is called before the first frame update
    void Start()
    {
        anit = GetComponent<Animator>();
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        hpSlider = GameObject.Find("HpSlider");
        SetHp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int _dmg)
    {
        if (isDead) return;
        hp -= _dmg;
        hpSlider.GetComponent<Slider>().value = hp;
        hpSlider.GetComponentInChildren<Text>().text = "HP " + hp.ToString();
        GameObject temp = Instantiate(dmgText, new Vector3(transform.position.x, transform.position.y + dmgTextYPos, transform.position.z), Quaternion.identity);
        temp.GetComponentInChildren<TextMesh>().text = "-" + _dmg.ToString();
        Destroy(temp, 0.3f);

        StartCoroutine(ChangeMaterial());

        if(hp <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        anit.SetTrigger("Death");
        GameManager.gm.GameOver();
    }

    IEnumerator ChangeMaterial()
    {
        smr.material = materials[1];

        yield return new WaitForSeconds(0.1f);

        smr.material = materials[0];
    }

    void SetHp()
    {
        hp = maxHP;

        hpSlider.GetComponent<Slider>().maxValue = hp;
        hpSlider.GetComponent<Slider>().value = hp;
        hpSlider.GetComponentInChildren<Text>().text = "HP " + hp.ToString();
    }

    public void GetHpItem(int _hp)
    {
        hp += _hp;
        StartCoroutine(PrintHpText("+", _hp));

        if (hp > maxHP) hp = maxHP;

        hpSlider.GetComponent<Slider>().value = hp;
    }

    public void GetMaxHpItem()
    {
        maxHP += 5;
        
        StartCoroutine(PrintHpText("MAX HP+", 5));

        if (maxHP >= limitMaxHP) maxHP = limitMaxHP;

        hpSlider.GetComponent<Slider>().maxValue += 5;
    }

    IEnumerator PrintHpText(string text, int _hp)
    {
        hpSlider.GetComponentInChildren<Text>().text = text + _hp.ToString();

        yield return new WaitForSeconds(1);

        hpSlider.GetComponentInChildren<Text>().text = "HP " + hp.ToString();
    }

}
