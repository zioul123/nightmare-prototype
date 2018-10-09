using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterTile : Tile
{
    // All the sprites
    [SerializeField]
    private Sprite[] waterSprites;

    // To view the sprite in the Tile Palette
    [SerializeField]
    private Sprite previewSprite;

    // Used when putting down a tile. Refresh the tile itself, and its neighbours.
    // Only need to refresh its immediate neighbours, tiles only influence those.
    // This calls GetTileData.
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++) {
                Vector3Int neighbourPosition = new Vector3Int(position.x + x, position.y + y, position.z);

                // Update any watertiles
                if (HasWater(tilemap, neighbourPosition))
                {
                    tilemap.RefreshTile(neighbourPosition);
                }
            }
        }


        base.RefreshTile(position, tilemap);
    }

    // Used to change the tiles.
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        uint composition = 0;

        // Generate bitmap of the surrounding tiles, 1 = water, 0 = nowater, order:
        // 7 6 5
        // 4   3
        // 2 1 0
        for (int y = 1; y >= -1; y--) { 
            for (int x = -1; x <= 1; x++) {
                if (x == 0 && y == 0) continue; // Skip itself

                if (HasWater(tilemap, new Vector3Int(position.x + x, position.y + y, position.z))) {
                    composition = composition << 1 | 1;
                } else {
                    composition = composition << 1;
                }
            }
        }

        tileData.sprite = SelectSprite(composition);

        // base.GetTileData(position, tilemap, ref tileData);
    }

    // Select the sprite based on the bitmask
    private Sprite SelectSprite(uint x) 
    {
        // Check if there are four adjacent ground (1,3,4,6)        X0X00X0X
        if ((x & (1 << 1)) == 0 && (x & (1 << 3)) == 0 && (x & (1 << 4)) == 0 && (x & (1 << 6)) == 0) {
            return waterSprites[0];
        }

        // Three adjacent ground
        // Bottom is water.     X0X00X1X
        if ((x & (1 << 1)) != 0 && (x & (1 << 3)) == 0 && (x & (1 << 4)) == 0 && (x & (1 << 6)) == 0) {
            return waterSprites[30];
        // Right is water.      X0X01X0X
        } else if ((x & (1 << 1)) == 0 && (x & (1 << 3)) != 0 && (x & (1 << 4)) == 0 && (x & (1 << 6)) == 0) {
            return waterSprites[26];
        // Left is water.       X0X10X0X
        } else if ((x & (1 << 1)) == 0 && (x & (1 << 3)) == 0 && (x & (1 << 4)) != 0 && (x & (1 << 6)) == 0) {
            return waterSprites[25];
        // Top is water.        X1X00X0X
        } else if ((x & (1 << 1)) == 0 && (x & (1 << 3)) == 0 && (x & (1 << 4)) == 0 && (x & (1 << 6)) != 0) {
            return waterSprites[29];
        }

        // Two adjacent ground
        // Bottom/Right is water        X0X01X1X
        if ((x & (1 << 1)) != 0 && (x & (1 << 3)) != 0 && (x & (1 << 4)) == 0 && (x & (1 << 6)) == 0) {
            uint t_x = x & 1;
            return t_x != 0 ? waterSprites[1] : waterSprites[2];
        // Right/Top is water           X1X01X0X
        } else if ((x & (1 << 1)) == 0 && (x & (1 << 3)) != 0 && (x & (1 << 4)) == 0 && (x & (1 << 6)) != 0) {
            uint t_x = x & (1 << 5);
            return t_x != 0 ? waterSprites[17] : waterSprites[18];
        // Top/Left is water            X1X10X0X
        } else if ((x & (1 << 1)) == 0 && (x & (1 << 3)) == 0 && (x & (1 << 4)) != 0 && (x & (1 << 6)) != 0) {
            uint t_x = x & (1 << 7);
            return t_x != 0 ? waterSprites[23] : waterSprites[24];
        // Left/Bottom is water         X0X10X1X
        } else if ((x & (1 << 1)) != 0 && (x & (1 << 3)) == 0 && (x & (1 << 4)) != 0 && (x & (1 << 6)) == 0) {
            uint t_x = x & (1 << 2);
            return t_x != 0 ? waterSprites[4] : waterSprites[5];
        // Bottom/Top is water          X1X00X1X
        } else if ((x & (1 << 1)) != 0 && (x & (1 << 3)) == 0 && (x & (1 << 4)) == 0 && (x & (1 << 6)) != 0) {
            return waterSprites[28];
        // Right/Left is water          X0X11X0X
        } else if ((x & (1 << 1)) == 0 && (x & (1 << 3)) != 0 && (x & (1 << 4)) != 0 && (x & (1 << 6)) == 0) {
            return waterSprites[27];
        }

        // One adjacent ground
        // Top is ground                X0X11X1X
        if ((x & (1 << 1)) != 0 && (x & (1 << 3)) != 0 && (x & (1 << 4)) != 0 && (x & (1 << 6)) == 0) {
            uint t_x = x & 31; // 00011111. Guaranteed to be 11X1X
            switch (t_x)
            {
                case 26: return waterSprites[16];   // 11010
                case 27: return waterSprites[6];    // 11011
                case 30: return waterSprites[14];   // 11110
                case 31: return waterSprites[3];    // 11111
            }
        // Right is ground              X1X10X1X
        } else if ((x & (1 << 1)) != 0 && (x & (1 << 3)) == 0 && (x & (1 << 4)) != 0 && (x & (1 << 6)) != 0) {
            // 192 is last two digits, 16 is 4th digit, 6 is 2nd and 3rd digits.
            uint t_x = (x & 192) >> 3 | (x & 16) >> 2 | (x & 6) >> 1; // 11_1_11_. Guaranteed to be X11X1.
            switch (t_x)
            {
                case 13: return waterSprites[15];   // 01101
                case 15: return waterSprites[13];   // 01111
                case 29: return waterSprites[11];   // 11101
                case 31: return waterSprites[12];   // 11111
            }
        // Left is ground               X1X01X1X
        } else if ((x & (1 << 1)) != 0 && (x & (1 << 3)) != 0 && (x & (1 << 4)) == 0 && (x & (1 << 6)) != 0) {
            // 96 is 6th and 7th digit, 8 is 3rd digit, 3 is 1st two digits.
            uint t_x = (x & 96) >> 2 | (x & 8) >> 1 | (x & 3); // _11_1_11. Guaranteed to be 1X11X.
            switch (t_x)
            {
                case 22: return waterSprites[10];   // 10110
                case 23: return waterSprites[9];    // 10111
                case 30: return waterSprites[8];    // 11110
                case 31: return waterSprites[7];    // 11111
            }
        // Bottom is ground             X1X11X0X
        } else if ((x & (1 << 1)) == 0 && (x & (1 << 3)) != 0 && (x & (1 << 4)) != 0 && (x & (1 << 6)) != 0) {
            uint t_x = (x & 248) >> 3; // 11111000. Guaranteed to be X1X11
            switch (t_x)
            {
                case 11: return waterSprites[22];   // 01011
                case 15: return waterSprites[21];   // 01111
                case 27: return waterSprites[20];   // 11011
                case 31: return waterSprites[19];   // 11111
            }
        }

        // All adjacent water
        switch (x) {
            case 250: return waterSprites[31];
            case 91: return waterSprites[32];
            case 123: return waterSprites[33];
            case 254: return waterSprites[34];
            case 95: return waterSprites[35];
            case 223: return waterSprites[36];
            case 251: return waterSprites[37];
            case 127: return waterSprites[38];
            case 219: return waterSprites[39];
            case 218: return waterSprites[40];
            case 222: return waterSprites[41];
            case 126: return waterSprites[42];
            case 122: return waterSprites[43];
            case 94: return waterSprites[44];
            case 90: return waterSprites[45];
        }

        // All diagonals and adjacent are water
        int randInt = UnityEngine.Random.Range(0, 100);
        if (randInt < 15) return waterSprites[46]; // 15% chance for lilypad
        else if (randInt < 35) return waterSprites[47]; // 20% chance for stale
        else return waterSprites[47]; // Otherwise have wave
    }

    // TODO: Not implemented
    public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);
    }

    // Check if the position on the tilemap is a WaterTile
    private bool HasWater(ITilemap tilemap, Vector3Int position) 
    {
        return tilemap.GetTile(position) == this;
    }

    // Add to the options menu in Unity
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/WaterTile")]
    public static void CreateWaterTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Watertile", "New Watertile", "asset", "Save watertile", "Assets");
        if (path == "") 
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WaterTile>(), path);
    }
#endif
}
