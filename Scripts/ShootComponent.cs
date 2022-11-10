using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootComponent : MonoBehaviour
{
    [SerializeField]
    int atk = 20;
    [SerializeField]
    int maxAtk = 50;
    [SerializeField]
    float shootRate = 0.2f;
    [SerializeField]
    float maxRate = 0.1f;
    float range = 100;
    float time = 0;
    int critical = 5;
    [SerializeField]
    int maxCri = 15;

    Ray shootRay;
    RaycastHit hit;
    int layerMask;

    LineRenderer shootLine;
    Coroutine cor;
    Light light;

    [SerializeField]
    float lightInten = 5;
    [SerializeField]
    float lineWidth = 0.1f;

    [SerializeField]
    GameObject fx;
    GameObject statUI;

    int atkLv = 1;
    int atkRateLv = 1;
    int CriLv = 1;

    AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        shootLine = GetComponent<LineRenderer>();
        light = GetComponent<Light>();
        layerMask = LayerMask.GetMask("Enemy");
        statUI = GameObject.Find("StatUI");
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gm.START || GameManager.gm.END) return;

        time += Time.deltaTime;

        if(Input.GetButton("Fire1") && time >= shootRate)
        {
            Shoot();
        }

        EffectScaleDown();
    }

    void Shoot()
    {
        light.intensity = lightInten;
        shootLine.startWidth = lineWidth;
        light.enabled = true;
        shootLine.enabled = true;
        shootLine.SetPosition(0, transform.position);   // Line Renderer's starting position

        shootRay.origin = transform.position;           // Light's starting position
        shootRay.direction = transform.forward;         // Direction of the light

        Instantiate(fx, transform.position, Quaternion.identity);
        sound.Play();

        if(Physics.Raycast(shootRay, out hit, range))
        {
            shootLine.SetPosition(1, hit.point);        // Line Renderer's line to hit enemy

            

            if(hit.transform.CompareTag("Enemy"))
            {
                int num = Random.Range(0, 100);

                if (critical < num)
                {
                    hit.transform.GetComponent<EnemyComponent>().TakeDamage(atk, false);
                }
                else
                {
                    hit.transform.GetComponent<EnemyComponent>().TakeDamage(atk * Random.Range(2, 4), true);
                }
            }
        }
        else
        {
            shootLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }

        time = 0;

        if (cor == null) cor = StartCoroutine(DisableEffect());
        else
        {
            StopCoroutine(cor);
            cor = StartCoroutine(DisableEffect());
        }
    }

    IEnumerator DisableEffect()
    {
        yield return new WaitForSeconds(0.1f);
        shootLine.enabled = false;
        light.enabled = false;
    }

    void EffectScaleDown()
    {
        if (!light.enabled) return;

        light.intensity = Mathf.Lerp(light.intensity, 0, 10f * Time.deltaTime);
        shootLine.startWidth = Mathf.Lerp(shootLine.startWidth, 0, 10f * Time.deltaTime);
    }

    public void GetAttackUpItem()
    {
        if (atkLv < 6) atkLv++;
        else return;

        atk = DataManager.dm.GetDataDamage(atkLv - 1);
        
        statUI.transform.GetChild(0).GetComponentInChildren<Text>().text = "Lv." + atkLv.ToString();
        /*if (atk >= maxAtk)
        {
            atk = maxAtk;
            statUI.transform.GetChild(0).GetComponentInChildren<Text>().text = "Lv.MAX";
        }*/
    }

    public void GetAttackSpeedItem()
    {
        if (atkRateLv < 6) atkRateLv++;
        else return;
        shootRate = DataManager.dm.GetDataAttackSpeed(atkRateLv - 1);
        
        statUI.transform.GetChild(1).GetComponentInChildren<Text>().text = "Lv." + atkRateLv.ToString();

       /* if (shootRate <= maxRate)
        {
            shootRate = maxRate;
            statUI.transform.GetChild(1).GetComponentInChildren<Text>().text = "Lv.MAX";

        }*/
    }

    public void GetCriticalItem()
    {
        critical++;
        CriLv++;
        statUI.transform.GetChild(2).GetComponentInChildren<Text>().text = "Lv." + CriLv.ToString();
        if (critical >= maxCri)
        {
            critical = maxCri;
            statUI.transform.GetChild(2).GetComponentInChildren<Text>().text = "Lv.MAX";
        }
    }

}
