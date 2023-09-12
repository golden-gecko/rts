using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class Indicators : MonoBehaviour
{
    void Awake()
    {
        bar = transform.Find("Bar");
        bar.gameObject.SetActive(true);

        barAmmunition = bar.Find("Ammunition").GetComponent<Image>();
        barArmour = bar.Find("Armour").GetComponent<Image>();
        barHealth = bar.Find("Health").GetComponent<Image>();
        barFuel = bar.Find("Fuel").GetComponent<Image>();
        barShield = bar.Find("Shield").GetComponent<Image>();

        range = transform.Find("Range");
        range.gameObject.SetActive(true);

        rangeGun = range.Find("Gun");
        rangePower = range.Find("Power");
        rangeRadar = range.Find("Radar");
        rangeSight = range.Find("Sight");

        construction = transform.Find("Construction");
        error = transform.Find("Error");
        selection = transform.Find("Selection");
        trace = transform.Find("Trace");
    }

    void Update()
    {
        MyGameObject myGameObject = GetComponentInParent<MyGameObject>();

        Vector3 scale = myGameObject.Scale;
        Vector3 size = myGameObject.Size;

        bar.transform.localPosition = new Vector3(0.0f, size.y, 0.0f);
        bar.transform.localScale = new Vector3(Mathf.Max(size.x, 1.0f), 1.0f, 1.0f);
        bar.transform.LookAt(Camera.main.transform.position);
        bar.transform.Rotate(0.0f, 180.0f, 0.0f);

        // Update ammunition.
        float gunPercent = 0.0f;

        foreach (Gun gun in GetComponents<Gun>())
        {
            gunPercent += gun.Ammunition.Percent;
            gunPercent *= 0.5f;
        }

        if (GetComponents<Gun>().Length > 0)
        {
            barAmmunition.fillAmount = gunPercent;
            barAmmunition.gameObject.SetActive(true);
        }
        else
        {
            barAmmunition.gameObject.SetActive(false);
        }

        // Update armour.
        Armour armour = myGameObject.GetComponent<Armour>();

        if (armour != null)
        {
            barArmour.fillAmount = armour.Value.Percent;
            barArmour.gameObject.SetActive(true);
        }
        else
        {
            barArmour.gameObject.SetActive(false);
        }

        // Update fuel.
        float enginePercent = 0.0f;

        foreach (Engine engine in GetComponents<Engine>())
        {
            enginePercent += engine.Fuel.Percent;
            enginePercent *= 0.5f;
        }

        if (GetComponents<Engine>().Length > 0)
        {
            barFuel.fillAmount = enginePercent;
            barFuel.gameObject.SetActive(true);
        }
        else
        {
            barFuel.gameObject.SetActive(false);
        }

        // Update health.
        barHealth.fillAmount = myGameObject.Health.Percent;
        barHealth.gameObject.SetActive(true);

        // Update shield.
        Shield shield = myGameObject.GetComponent<Shield>();

        if (shield != null)
        {
            barShield.fillAmount = shield.Capacity.Percent;
            barShield.gameObject.SetActive(true);
        }
        else
        {
            barShield.gameObject.SetActive(false);
        }

        // Update trace.
        float radius = myGameObject.Radius;

        trace.localScale = new Vector3(radius / scale.x, radius / scale.y, radius / scale.z);
    }

    public void OnConstruction()
    {
        MyGameObject myGameObject = GetComponentInParent<MyGameObject>();

        Vector3 size = myGameObject.Size;

        bar.gameObject.SetActive(false);
        range.gameObject.SetActive(false);

        construction.gameObject.SetActive(true);
        construction.transform.localPosition = new Vector3(0.0f, size.y * 0.5f, 0.0f);
        construction.transform.localScale = size;
    }

    public void OnConstructionEnd()
    {
        bar.gameObject.SetActive(true);
        range.gameObject.SetActive(true);

        construction.gameObject.SetActive(false);
    }

    public void OnError()
    {
        MyGameObject myGameObject = GetComponentInParent<MyGameObject>();

        Vector3 size = myGameObject.Size;

        error.gameObject.SetActive(true);
        error.transform.localPosition = new Vector3(0.0f, size.y * 0.5f, 0.0f);
        error.transform.localScale = size;
    }

    public void OnErrorEnd()
    {
        error.gameObject.SetActive(false);
    }

    public void OnHide()
    {
        bar.gameObject.SetActive(false);
        construction.gameObject.SetActive(false);
        range.gameObject.SetActive(false);
        trace.gameObject.SetActive(false);
    }

    public void OnPlayerChange(Player player)
    {
        if (player != null)
        {
            selection.GetComponent<SpriteRenderer>().sprite = player.SelectionSprite;
        }
    }

    public void OnRadar()
    {
        bar.gameObject.SetActive(false);
        construction.gameObject.SetActive(false);
        range.gameObject.SetActive(false);
        trace.gameObject.SetActive(true);
    }

    public void OnSelect(bool status)
    {
        MyGameObject myGameObject = GetComponentInParent<MyGameObject>();

        Vector3 scale = myGameObject.Scale;
        Vector3 size = myGameObject.Size;

        Gun gun = myGameObject.GetComponent<Gun>();

        if (gun != null)
        {
            rangeGun.gameObject.SetActive(status);
            rangeGun.localScale = new Vector3(gun.Range * 2.0f / scale.x, gun.Range * 2.0f / scale.z, 1.0f);
        }

        PowerPlant powerPlant = myGameObject.GetComponent<PowerPlant>();

        if (powerPlant != null)
        {
            rangePower.gameObject.SetActive(status);
            rangePower.localScale = new Vector3(powerPlant.Range * 2.0f / scale.x, powerPlant.Range * 2.0f / scale.z, 1.0f);
        }

        Radar radar = myGameObject.GetComponent<Radar>();

        if (radar != null)
        {
            rangeRadar.gameObject.SetActive(status);
            rangeRadar.localScale = new Vector3(radar.Range * 2.0f / scale.x, radar.Range * 2.0f / scale.z, 1.0f);
        }

        Sight sight = myGameObject.GetComponent<Sight>();

        if (sight != null)
        {
            rangeSight.gameObject.SetActive(status);
            rangeSight.localScale = new Vector3(sight.Range * 2.0f / scale.x, sight.Range * 2.0f / scale.z, 1.0f);
        }

        selection.gameObject.SetActive(status);
        selection.localScale = new Vector3(size.x * 1.1f, size.z * 1.1f, 1.0f);
    }

    public void OnShow()
    {
        MyGameObject myGameObject = GetComponentInParent<MyGameObject>();

        switch (myGameObject.State)
        {
            case MyGameObjectState.UnderAssembly:
            case MyGameObjectState.UnderConstruction:
                bar.gameObject.SetActive(false);
                construction.gameObject.SetActive(true);
                range.gameObject.SetActive(false);
                trace.gameObject.SetActive(false);
                break;

            case MyGameObjectState.Operational:
                bar.gameObject.SetActive(true);
                construction.gameObject.SetActive(false);
                range.gameObject.SetActive(true);
                trace.gameObject.SetActive(false);
                break;
        }
    }

    private Transform bar;
    private Image barAmmunition;
    private Image barArmour;
    private Image barFuel;
    private Image barHealth;
    private Image barShield;

    private Transform range;
    private Transform rangeGun;
    private Transform rangePower;
    private Transform rangeRadar;
    private Transform rangeSight;

    private Transform construction;
    private Transform error;
    private Transform selection;
    private Transform trace;
}
