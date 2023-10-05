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
        selection = transform.Find("Selection");
        trace = transform.Find("Trace");
    }

    void Update()
    {
        MyGameObject myGameObject = GetComponentInParent<MyGameObject>();

        if (myGameObject.ShowIndicators)
        {
            gameObject.SetActive(true);

            UpdateBars(myGameObject);
            UpdateRange(myGameObject);
            UpdateOrders(myGameObject);
            UpdateSigns(myGameObject);

            UpdateConstruction(myGameObject);
            UpdateError(myGameObject);
            UpdateSelection(myGameObject);
            UpdateTrace(myGameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnConstruction()
    {
        bar.gameObject.SetActive(false);
        range.gameObject.SetActive(false);

        construction.gameObject.SetActive(true);
    }

    public void OnConstructionEnd()
    {
        bar.gameObject.SetActive(true);

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

    public void OnHide()
    {
        bar.gameObject.SetActive(false);
        range.gameObject.SetActive(false);

        construction.gameObject.SetActive(false);
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
        MyGameObject myGameObject = GetComponentInParent<MyGameObject>();

        if (HUD.Instance.ActivePlayer == myGameObject.Player || HUD.Instance.ActivePlayer.TechnologyTree.IsDiscovered("Radar 3"))
        {
            bar.gameObject.SetActive(true);
        }
        else
        {
            bar.gameObject.SetActive(false);
        }

        range.gameObject.SetActive(false);

        construction.gameObject.SetActive(false);
        trace.gameObject.SetActive(true);
    }

    public void OnSelect(bool status)
    {
        range.gameObject.SetActive(status);
        order.gameObject.SetActive(status);
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
                }

                trace.gameObject.SetActive(false);
                break;
        }
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
        Gun gun;

        if (myGameObject.TryGetComponent<Gun>(out gun))
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
        Armour armour;

        if (myGameObject.TryGetComponent<Armour>(out armour))
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
        Engine engine;

        if (myGameObject.TryGetComponent<Engine>(out engine))
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
        Shield shield;

        if (myGameObject.TryGetComponent<Shield>(out shield))
        {
            barShield.gameObject.SetActive(true);
            barShield.fillAmount = shield.Capacity.Percent;
        }
        else
        {
            barShield.gameObject.SetActive(false);
        }
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
        Gun gun;
        
        if (myGameObject.TryGetComponent(out gun))
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
        PowerPlant powerPlant;

        if (myGameObject.TryGetComponent(out powerPlant))
        {
            Vector3 scale = myGameObject.Scale;

            rangePower.gameObject.SetActive(true);
            rangePower.localScale = new Vector3(powerPlant.Range * 2.0f / scale.x, powerPlant.Range * 2.0f / scale.z, 1.0f);
        }
        else
        {
            rangePower.gameObject.SetActive(false);
        }
    }

    private void UpdateRangeRadar(MyGameObject myGameObject)
    {
        Radar radar;

        if (myGameObject.TryGetComponent(out radar))
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
        Sight sight;

        if (myGameObject.TryGetComponent(out sight))
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

        if (order == null)
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
        construction.transform.localScale = size * Config.Indicator.Margin;
    }

    private void UpdateError(MyGameObject myGameObject)
    {
        Vector3 size = myGameObject.Size;

        error.transform.localPosition = new Vector3(0.0f, size.y * 0.5f, 0.0f);
        error.transform.localScale = size * Config.Indicator.Margin;
    }

    private void UpdateSelection(MyGameObject myGameObject)
    {
        Vector3 size = myGameObject.Size;

        selection.localScale = new Vector3(size.x * Config.Indicator.Margin, size.z * Config.Indicator.Margin, 1.0f);
    }

    private void UpdateTrace(MyGameObject myGameObject)
    {
        Vector3 scale = myGameObject.Scale;

        if (HUD.Instance.ActivePlayer.TechnologyTree.IsDiscovered("Radar 2")) // TODO: Is ActivePlayer correct here?
        {
            float radius = myGameObject.Radius;

            trace.localScale = new Vector3(radius / scale.x, radius / scale.y, radius / scale.z);
        }
        else
        {
            float radius = 1.0f;

            trace.localScale = new Vector3(radius / scale.x, radius / scale.y, radius / scale.z);
        }
    }

    private Transform bar;
    private Image barAmmunition;
    private Image barArmour;
    private Image barFuel;
    private Image barHealth;
    private Image barShield;

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
    private Transform selection;
    private Transform trace;
}
