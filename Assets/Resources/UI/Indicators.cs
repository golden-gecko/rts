using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Indicators : MonoBehaviour
{
    void Awake()
    {
        bar = transform.Find("Bar");
        barAmmunition = bar.Find("Ammunition").GetComponent<Image>();
        barArmour = bar.Find("Armour").GetComponent<Image>();
        barHealth = bar.Find("Health").GetComponent<Image>();
        barFuel = bar.Find("Fuel").GetComponent<Image>();
        barShield = bar.Find("Shield").GetComponent<Image>();

        icon = transform.Find("Icon");
        iconDamageOn = icon.Find("Damage").Find("On").GetComponent<Image>();
        iconDamageOff = icon.Find("Damage").Find("Off").GetComponent<Image>();
        iconPowerOn = icon.Find("Power").Find("On").GetComponent<Image>();
        iconPowerOff = icon.Find("Power").Find("Off").GetComponent<Image>();
        iconResourceOn = icon.Find("Resource").Find("On").GetComponent<Image>();
        iconResourceOff = icon.Find("Resource").Find("Off").GetComponent<Image>();
        iconStateOn = icon.Find("State").Find("On").GetComponent<Image>();
        iconStateOff = icon.Find("State").Find("Off").GetComponent<Image>();
        iconWorkOn = icon.Find("Work").Find("On").GetComponent<Image>();
        iconWorkOff = icon.Find("Work").Find("Off").GetComponent<Image>();

        order = transform.Find("Order");
        orderLine = order.Find("Line").GetComponent<LineRenderer>();
        orderSphere = order.Find("Sphere");
        orderText = order.Find("Text");
        orderTextValue = orderText.Find("Value");
        orderTextValueMesh = orderTextValue.GetComponent<TextMeshProUGUI>();

        range = transform.Find("Range");
        rangeGun = range.Find("Gun");
        rangePower = range.Find("Power");
        rangeRadar = range.Find("Radar");
        rangeSight = range.Find("Sight");

        sign = transform.Find("Sign");
        signEntrance = sign.Find("Entrance");
        signExit = sign.Find("Exit");

        rangeGun = range.Find("Gun");
        rangePower = range.Find("Power");
        rangeRadar = range.Find("Radar");
        rangeSight = range.Find("Sight");

        construction = transform.Find("Construction");
        error = transform.Find("Error");
        exploration = transform.Find("Exploration");
        radar = transform.Find("Radar");
        selection = transform.Find("Selection");
    }

    void Update()
    {
        MyGameObject myGameObject = GetComponentInParent<MyGameObject>();

        if (myGameObject.ShowIndicators)
        {
            gameObject.SetActive(true);

            UpdateBars(myGameObject);
            UpdateIcons(myGameObject);
            UpdateOrders(myGameObject);
            UpdateRange(myGameObject);
            UpdateSigns(myGameObject);

            UpdateConstruction(myGameObject);
            UpdateError(myGameObject);
            UpdateExploration(myGameObject);
            UpdateRadar(myGameObject);
            UpdateSelection(myGameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnConstruction()
    {
        bar.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
        range.gameObject.SetActive(false);

        construction.gameObject.SetActive(true);
        sign.gameObject.SetActive(true);
    }

    public void OnConstructionEnd()
    {
        bar.gameObject.SetActive(true);
        icon.gameObject.SetActive(true);

        construction.gameObject.SetActive(false);
    }

    public void OnError()
    {
        error.gameObject.SetActive(true);
    }

    public void OnErrorEnd()
    {
        error.gameObject.SetActive(false);
    }

    public void OnExploration()
    {
        exploration.gameObject.SetActive(true);
    }

    public void OnHide()
    {
        bar.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
        range.gameObject.SetActive(false);

        construction.gameObject.SetActive(false);
        exploration.gameObject.SetActive(false);
        radar.gameObject.SetActive(false);
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
        MyGameObject myGameObject = GetComponentInParent<MyGameObject>();

        if (HUD.Instance.ActivePlayer == myGameObject.Player || HUD.Instance.ActivePlayer.TechnologyTree.IsDiscovered("Radar 3"))
        {
            bar.gameObject.SetActive(true);
        }
        else
        {
            bar.gameObject.SetActive(false);
        }

        icon.gameObject.SetActive(false);
        range.gameObject.SetActive(false);

        construction.gameObject.SetActive(false);
        exploration.gameObject.SetActive(false);
        radar.gameObject.SetActive(true);
    }

    public void OnSelect(bool status)
    {
        order.gameObject.SetActive(status);
        range.gameObject.SetActive(status && UI_Layers.Instance.range.value);
        sign.gameObject.SetActive(status);

        selection.gameObject.SetActive(status);
    }

    public void OnShow()
    {
        MyGameObject myGameObject = GetComponentInParent<MyGameObject>();

        gameObject.SetActive(myGameObject.ShowIndicators);

        switch (myGameObject.State)
        {
            case MyGameObjectState.UnderAssembly:
            case MyGameObjectState.UnderConstruction:
                OnConstruction();
                break;

            case MyGameObjectState.Operational:
                if (HUD.Instance.ActivePlayer == myGameObject.Player)
                {
                    bar.gameObject.SetActive(true);
                    icon.gameObject.SetActive(true);
                }

                exploration.gameObject.SetActive(false);
                radar.gameObject.SetActive(false);
                break;
        }
    }

    public void OnToggleRange(bool value)
    {
        range.gameObject.SetActive(value);
    }

    private void UpdateBars(MyGameObject myGameObject)
    {
        Vector3 size = myGameObject.Size;

        bar.transform.localPosition = new Vector3(0.0f, size.y, 0.0f);
        bar.transform.localScale = new Vector3(Mathf.Max(size.x, 1.0f), 1.0f, 1.0f);
        bar.transform.LookAt(Camera.main.transform.position);
        bar.transform.Rotate(0.0f, 180.0f, 0.0f);

        UpdateBarAmmunition(myGameObject);
        UpdateBarArmour(myGameObject);
        UpdateBarFuel(myGameObject);
        UpdateBarHealth(myGameObject);
        UpdateBarShield(myGameObject);
    }

    private void UpdateBarAmmunition(MyGameObject myGameObject)
    {
        if (myGameObject.TryGetComponent(out Gun gun))
        {
            barAmmunition.gameObject.SetActive(true);
            barAmmunition.fillAmount = gun.Ammunition.Percent;
        }
        else
        {
            barAmmunition.gameObject.SetActive(false);
        }
    }

    private void UpdateBarArmour(MyGameObject myGameObject)
    {
        if (myGameObject.TryGetComponent(out Armour armour))
        {
            barArmour.gameObject.SetActive(true);
            barArmour.fillAmount = armour.Value.Percent;
        }
        else
        {
            barArmour.gameObject.SetActive(false);
        }
    }

    private void UpdateBarFuel(MyGameObject myGameObject)
    {
        if (myGameObject.TryGetComponent(out Engine engine))
        {
            barFuel.gameObject.SetActive(true);
            barFuel.fillAmount = engine.Fuel.Percent;
        }
        else
        {
            barFuel.gameObject.SetActive(false);
        }
    }

    private void UpdateBarHealth(MyGameObject myGameObject)
    {
        barHealth.gameObject.SetActive(true);
        barHealth.fillAmount = myGameObject.Health.Percent;
    }

    private void UpdateBarShield(MyGameObject myGameObject)
    {
        if (myGameObject.TryGetComponent(out Shield shield))
        {
            barShield.gameObject.SetActive(true);
            barShield.fillAmount = shield.Capacity.Percent;
        }
        else
        {
            barShield.gameObject.SetActive(false);
        }
    }

    private void UpdateIcons(MyGameObject myGameObject)
    {
        Vector3 size = myGameObject.Size;

        icon.transform.localPosition = new Vector3(0.0f, size.y, 0.0f);
        icon.transform.LookAt(Camera.main.transform.position);
        icon.transform.Rotate(0.0f, 180.0f, 0.0f);

        UpdateIconDamage(myGameObject);
        UpdateIconPower(myGameObject);
        UpdateIconResource(myGameObject);
        UpdateIconState(myGameObject);
        UpdateIconWork(myGameObject);
    }

    private void UpdateIconDamage(MyGameObject myGameObject)
    {
        iconDamageOn.gameObject.SetActive(myGameObject.Health.Full == false);
        iconDamageOff.gameObject.SetActive(false);
    }

    private void UpdateIconPower(MyGameObject myGameObject)
    {
        if (myGameObject.TryGetComponent(out PowerPlant _))
        {
            iconPowerOn.gameObject.SetActive(myGameObject.Powered);
            iconPowerOff.gameObject.SetActive(myGameObject.Powered == false);
        }
    }

    private void UpdateIconResource(MyGameObject myGameObject)
    {
        if (myGameObject.TryGetComponent(out Storage storage))
        {
            float percent = storage.Resources.PercentSum;

            iconResourceOn.gameObject.SetActive(percent >= 0.5f);
            iconResourceOff.gameObject.SetActive(percent < 0.5f);
        }
        else
        {
            iconResourceOn.gameObject.SetActive(false);
            iconResourceOff.gameObject.SetActive(false);
        }
    }

    private void UpdateIconState(MyGameObject myGameObject)
    {
        iconStateOn.gameObject.SetActive(myGameObject.Enabled);
        iconStateOff.gameObject.SetActive(myGameObject.Enabled == false);
    }

    private void UpdateIconWork(MyGameObject myGameObject)
    {
        iconWorkOn.gameObject.SetActive(myGameObject.Working);
        iconWorkOff.gameObject.SetActive(myGameObject.Working == false);
    }

    private void UpdateRange(MyGameObject myGameObject)
    {
        UpdateRangeGun(myGameObject);
        UpdateRangePower(myGameObject);
        UpdateRangeRadar(myGameObject);
        UpdateRangeSight(myGameObject);
    }

    private void UpdateRangeGun(MyGameObject myGameObject)
    {
        if (myGameObject.TryGetComponent(out Gun gun))
        {
            Vector3 scale = myGameObject.Scale;

            rangeGun.gameObject.SetActive(true);
            rangeGun.localScale = new Vector3(gun.Range.Total * 2.0f / scale.x, gun.Range.Total * 2.0f / scale.z, 1.0f);

        }
        else
        {
            rangeGun.gameObject.SetActive(false);
        }
    }
    
    private void UpdateRangePower(MyGameObject myGameObject)
    {
        if (myGameObject.TryGetComponent(out PowerPlant powerPlant))
        {
            Vector3 scale = myGameObject.Scale;

            rangePower.gameObject.SetActive(true);
            rangePower.localScale = new Vector3(powerPlant.Range.Total * 2.0f / scale.x, powerPlant.Range.Total * 2.0f / scale.z, 1.0f);
        }
        else
        {
            rangePower.gameObject.SetActive(false);
        }
    }

    private void UpdateRangeRadar(MyGameObject myGameObject)
    {
        if (myGameObject.TryGetComponent(out Radar radar))
        {
            Vector3 scale = myGameObject.Scale;

            rangeRadar.gameObject.SetActive(true);
            rangeRadar.localScale = new Vector3(radar.Range.Total * 2.0f / scale.x, radar.Range.Total * 2.0f / scale.z, 1.0f);
        }
        else
        {
            rangeRadar.gameObject.SetActive(false);
        }
    }

    private void UpdateRangeSight(MyGameObject myGameObject)
    {
        if (myGameObject.TryGetComponent(out Sight sight))
        {
            Vector3 scale = myGameObject.Scale;

            rangeSight.gameObject.SetActive(true);
            rangeSight.localScale = new Vector3(sight.Range.Total * 2.0f / scale.x, sight.Range.Total * 2.0f / scale.z, 1.0f);
        }
        else
        {
            rangeSight.gameObject.SetActive(false);
        }
    }

    private void UpdateOrders(MyGameObject myGameObject)
    {
        orderLine.gameObject.SetActive(false);
        orderSphere.gameObject.SetActive(false);
        orderText.gameObject.SetActive(false);
        orderTextValue.transform.LookAt(Camera.main.transform.position);
        orderTextValue.transform.Rotate(0.0f, 180.0f, 0.0f);

        Order order = myGameObject.Orders.Items.Where(x => x.Type != OrderType.Wait).FirstOrDefault();

        if (order == null || order.IsValid() == false)
        {
            return;
        }

        Vector3 size = myGameObject.Size;

        switch (order.Type)
        {
            case OrderType.Assemble:
                orderText.gameObject.SetActive(true);
                orderText.transform.localPosition = new Vector3(orderText.transform.localPosition.x, size.y * Config.Indicator.TextOffset, orderText.transform.localPosition.z);
                orderTextValueMesh.SetText(string.Format("Assembling\n{0}", order.Prefab));
                break;

            case OrderType.Attack:
            case OrderType.Construct:
            case OrderType.Follow:
            case OrderType.Gather:
            case OrderType.Guard:
            case OrderType.Move:
            case OrderType.Patrol:
            case OrderType.Rally:
                Vector3 position = order.IsTargetGameObject && order.TargetGameObject != null ? order.TargetGameObject.Position : order.TargetPosition;

                orderLine.gameObject.SetActive(true);
                orderLine.useWorldSpace = true;
                orderLine.SetPositions(new Vector3[] { myGameObject.Position, position });

                orderSphere.gameObject.SetActive(true);
                orderSphere.transform.position = position;
                break;

            case OrderType.Load:
                orderLine.gameObject.SetActive(true);
                orderLine.useWorldSpace = true;
                orderLine.SetPositions(new Vector3[] { myGameObject.Position, order.SourceGameObject.Position });

                orderSphere.gameObject.SetActive(true);
                orderSphere.transform.position = order.SourceGameObject.Position;
                break;

            case OrderType.Produce:
                orderText.gameObject.SetActive(true);
                orderText.transform.localPosition = new Vector3(orderText.transform.localPosition.x, size.y * Config.Indicator.TextOffset, orderText.transform.localPosition.z);
                orderTextValueMesh.SetText(string.Format("Producing\n{0}", order.Recipe));
                break;

            case OrderType.Research:
                orderText.gameObject.SetActive(true);
                orderText.transform.localPosition = new Vector3(orderText.transform.localPosition.x, size.y * Config.Indicator.TextOffset, orderText.transform.localPosition.z);
                orderTextValueMesh.SetText(string.Format("Researching\n{0}", order.Technology));
                break;

            case OrderType.Transport:
                orderLine.gameObject.SetActive(true);
                orderLine.useWorldSpace = true;
                orderLine.SetPositions(new Vector3[] { myGameObject.Position, order.SourceGameObject.Position, order.TargetGameObject.Position });

                orderSphere.gameObject.SetActive(true);
                orderSphere.transform.position = order.TargetGameObject.Position;
                break;

            case OrderType.Unload:
                orderLine.gameObject.SetActive(true);
                orderLine.useWorldSpace = true;
                orderLine.SetPositions(new Vector3[] { myGameObject.Position, order.TargetGameObject.Position });

                orderSphere.gameObject.SetActive(true);
                orderSphere.transform.position = order.TargetGameObject.Position;
                break;
        }
    }

    private void UpdateSigns(MyGameObject myGameObject)
    {
        signEntrance.gameObject.SetActive(myGameObject.ShowEntrance);
        signEntrance.position = myGameObject.Entrance;

        signExit.gameObject.SetActive(myGameObject.ShowExit);
        signExit.position = myGameObject.Exit;
    }

    private void UpdateConstruction(MyGameObject myGameObject)
    {
        Vector3 size = myGameObject.Size;

        construction.transform.localPosition = new Vector3(0.0f, size.y * 0.5f, 0.0f);
        construction.transform.localScale = size;
    }

    private void UpdateError(MyGameObject myGameObject)
    {
        Vector3 size = myGameObject.Size;

        error.transform.localPosition = new Vector3(0.0f, size.y * 0.5f, 0.0f);
        error.transform.localScale = size;
    }

    private void UpdateExploration(MyGameObject myGameObject)
    {
        Vector3 size = myGameObject.Size;

        exploration.transform.localPosition = new Vector3(0.0f, size.y * 0.5f, 0.0f);
        exploration.transform.localScale = size;
    }

    private void UpdateRadar(MyGameObject myGameObject)
    {
        Vector3 scale = myGameObject.Scale;

        if (HUD.Instance.ActivePlayer.TechnologyTree.IsDiscovered("Radar 2"))
        {
            float radius = myGameObject.Radius;

            radar.localScale = new Vector3(radius / scale.x, radius / scale.y, radius / scale.z);
        }
        else
        {
            float radius = 1.0f;

            radar.localScale = new Vector3(radius / scale.x, radius / scale.y, radius / scale.z);
        }
    }

    private void UpdateSelection(MyGameObject myGameObject)
    {
        Vector3 size = myGameObject.Size;

        selection.localScale = new Vector3(size.x * Config.Indicator.Margin, size.z * Config.Indicator.Margin, 1.0f);
    }

    private Transform bar;
    private Image barAmmunition;
    private Image barArmour;
    private Image barFuel;
    private Image barHealth;
    private Image barShield;

    private Transform icon;
    private Image iconDamageOn;
    private Image iconDamageOff;
    private Image iconPowerOn;
    private Image iconPowerOff;
    private Image iconResourceOn;
    private Image iconResourceOff;
    private Image iconStateOn;
    private Image iconStateOff;
    private Image iconWorkOn;
    private Image iconWorkOff;

    private Transform order;
    private LineRenderer orderLine;
    private Transform orderSphere;
    private Transform orderText;
    private Transform orderTextValue;
    private TextMeshProUGUI orderTextValueMesh;

    private Transform range;
    private Transform rangeGun;
    private Transform rangePower;
    private Transform rangeRadar;
    private Transform rangeSight;

    private Transform sign;
    private Transform signEntrance;
    private Transform signExit;

    private Transform construction;
    private Transform error;
    private Transform exploration;
    private Transform radar;
    private Transform selection;
}
