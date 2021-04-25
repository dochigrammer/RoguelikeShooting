using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    public int RowCount = 20;
    public int ColCount = 20;

    public float GridSize = 1.0f;

    public GameObject[] Tiles;
    public GameObject[] Objects;
    public GameObject[] Enemies;

    private void Start()
    {
        InitlaizeLevel();
    }

    void InitlaizeLevel()
    {
        int half_row_count = (int)(RowCount * 0.5f);
        int half_col_count = (int)(ColCount * 0.5f);

        // Row와 Col의 입력으로 타일들을 생성함.
        for (int row = 0; row < RowCount; ++row)
        {
            for (int col = 0; col < ColCount; ++col)
            {
                // 타일 중 랜덤한 타일로
                var tile = Instantiate(GetRandomTile());

                // 위치 조절
                tile.transform.position = new Vector3(half_row_count * -GridSize + row * GridSize, 0.0f, half_col_count * -GridSize + col * GridSize);

                // 랜덤 회전
                tile.transform.rotation = Quaternion.Euler(new Vector3(0.0f, Random.Range(0, 4) * 90.0f, 0.0f));

                // 적군들의 AI를 위해 새로 생성된 타일들을 NavMesh Build
                var nav_mesh_surface = tile.GetComponent<NavMeshSurface>();
                if (nav_mesh_surface != null)
                {
                    nav_mesh_surface.BuildNavMesh();
                }

                // 해당위치에 적 혹은 레벨 오브젝트를 생성할지 말지 결정
                GameObject random_object = GetRandomObject();

                // 해당 객체가 랜덤으로 존재할경우
                if (random_object != null)
                {
                    var spawned_object = Instantiate(random_object);

                    if (spawned_object != null)
                    {
                        spawned_object.transform.position = tile.transform.position + Vector3.up * 1.0f;
                    }
                }
            }
        }
    }

    protected GameObject GetRandomObject()
    {
        if (Enemies.Length > 0 && Random.Range(0, 16) == 0)
        {
            return Enemies[Random.Range(0, Enemies.Length - 1)];
        }

        if (Objects.Length > 0 && Random.Range(0, 8) == 0)
        {
            return Objects[Random.Range(0, Objects.Length - 1)];
        }

        return null;
    }

    protected GameObject GetRandomTile()
    {
        var random_index = Random.Range(0, Tiles.Length - 1);

        return Tiles[random_index];
    }

}
