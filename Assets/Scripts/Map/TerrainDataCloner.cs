using UnityEngine;

public class TerrainDataCloner
{
    public static TerrainData Clone(TerrainData original)
    {
        TerrainData dup = new TerrainData();

        dup.alphamapResolution = original.alphamapResolution;
        dup.baseMapResolution = original.baseMapResolution;

        dup.detailPrototypes = CloneDetailPrototypes(original.detailPrototypes);

        // The resolutionPerPatch is not publicly accessible so
        // it can not be cloned properly, thus the recommendet default
        // number of 16
        dup.SetDetailResolution(original.detailResolution, 16);

        dup.heightmapResolution = original.heightmapResolution;
        dup.size = original.size;

        //dup.splatPrototypes = CloneSplatPrototypes(original.splatPrototypes);

        dup.terrainLayers = original.terrainLayers;

        dup.wavingGrassAmount = original.wavingGrassAmount;
        dup.wavingGrassSpeed = original.wavingGrassSpeed;
        dup.wavingGrassStrength = original.wavingGrassStrength;
        dup.wavingGrassTint = original.wavingGrassTint;

        dup.SetAlphamaps(0, 0, original.GetAlphamaps(0, 0, original.alphamapWidth, original.alphamapHeight));
        dup.SetHeights(0, 0, original.GetHeights(0, 0, original.heightmapResolution, original.heightmapResolution));

        for (int n = 0; n < original.detailPrototypes.Length; n++)
        {
            dup.SetDetailLayer(0, 0, n, original.GetDetailLayer(0, 0, original.detailWidth, original.detailHeight, n));
        }

        dup.treePrototypes = CloneTreePrototypes(original.treePrototypes);
        dup.treeInstances = CloneTreeInstances(original.treeInstances);

        return dup;
    }

    static DetailPrototype[] CloneDetailPrototypes(DetailPrototype[] original)
    {
        DetailPrototype[] protoDuplicate = new DetailPrototype[original.Length];

        for (int n = 0; n < original.Length; n++)
        {
            protoDuplicate[n] = new DetailPrototype
            {
                dryColor = original[n].dryColor,
                healthyColor = original[n].healthyColor,
                maxHeight = original[n].maxHeight,
                maxWidth = original[n].maxWidth,
                minHeight = original[n].minHeight,
                minWidth = original[n].minWidth,
                noiseSpread = original[n].noiseSpread,
                prototype = original[n].prototype,
                prototypeTexture = original[n].prototypeTexture,
                renderMode = original[n].renderMode,
                usePrototypeMesh = original[n].usePrototypeMesh,
                useInstancing = original[n].useInstancing,
            };
        }

        return protoDuplicate;
    }

    static TreePrototype[] CloneTreePrototypes(TreePrototype[] original)
    {
        TreePrototype[] protoDuplicate = new TreePrototype[original.Length];

        for (int n = 0; n < original.Length; n++)
        {
            protoDuplicate[n] = new TreePrototype
            {
                bendFactor = original[n].bendFactor,
                prefab = original[n].prefab,
            };
        }

        return protoDuplicate;
    }

    static TreeInstance[] CloneTreeInstances(TreeInstance[] original)
    {
        TreeInstance[] treeInst = new TreeInstance[original.Length];

        System.Array.Copy(original, treeInst, original.Length);

        return treeInst;
    }
}
