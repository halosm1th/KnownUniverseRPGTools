namespace KnownUniversePoliticsGameWebApp.Data;

public interface IKUPCombatAsset : IKUPAsset
{
    int HP { get; }
    CombatAssetSize Size { get; }

    int IKUPAsset.MoralIncome => 0;
    int IKUPAsset.MoneyIncome => 0;

    int IKUPAsset.UpKeepCost
    {
        get
        {
            if (Size == CombatAssetSize.Small)
            {
                return 10;
            }
            else if (Size == CombatAssetSize.Medium)
            {
                if (HP == 2)
                {
                    return 20;
                }

                if (HP == 1)
                {
                    return 10;
                }
            }
            else if (Size == CombatAssetSize.Large)
            {
                if (HP == 4)
                {
                    return 50;
                }

                if (HP == 3)
                {
                    return 35;
                }

                if (HP == 2)
                {
                    return 20;
                }

                if (HP == 1)
                {
                    return 10;
                }
            }

            return 5;
        }
    }

    int IKUPAsset.MoralCost
    {
        get
        {
            if (Size == CombatAssetSize.Small)
            {
                return 5;
            }else if (Size == CombatAssetSize.Medium)
            {
                if (HP == 2)
                {
                    return 10;   
                }

                if (HP == 1)
                {
                    return 15;
                }
            }else if (Size == CombatAssetSize.Large)
            {
                if (HP == 4)
                {
                    return 15;
                }
                
                if (HP == 3)
                {
                    return 20;   
                }
                if (HP == 2)
                {
                    return 25;   
                }

                if (HP == 1)
                {
                    return 30;
                }
            }

            return 5;
        }
    }
}