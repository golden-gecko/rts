public class JobHandlerTransport : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        foreach (ResourceRequest consumer in myGameObject.Player.Consumers.Items)
        {
            if (consumer.MyGameObject == null)
            {
                continue;
            }

            if (consumer.MyGameObject == myGameObject)
            {
                continue;
            }

            foreach (ResourceRequest producer in myGameObject.Player.Producers.Items)
            {
                if (producer.MyGameObject == null)
                {
                    continue;
                }

                if (producer.MyGameObject == myGameObject)
                {
                    continue;
                }

                if (producer.MyGameObject == consumer.MyGameObject)
                {
                    continue;
                }

                if (producer.Direction == consumer.Direction)
                {
                    continue;
                }

                if (producer.Name != consumer.Name)
                {
                    continue;
                }

                myGameObject.Player.Consumers.MoveToEnd();
                myGameObject.Player.Producers.MoveToEnd();

                return Order.Transport(producer.MyGameObject, consumer.MyGameObject, producer.Name, producer.Value);
            }
        }

        return null;
    }
}
