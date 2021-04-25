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

        // Row�� Col�� �Է����� Ÿ�ϵ��� ������.
        for (int row = 0; row < RowCount; ++row)
        {
            for (int col = 0; col < ColCount; ++col)
            {
                // Ÿ�� �� ������ Ÿ�Ϸ�
                var tile = Instantiate(GetRandomTile());

                // ��ġ ����
                tile.transform.position = new Vector3(half_row_count * -GridSize + row * GridSize, 0.0f, half_col_count * -GridSize + col * GridSize);

                // ���� ȸ��
                tile.transform.rotation = Quaternion.Euler(new Vector3(0.0f, Random.Range(0, 4) * 90.0f, 0.0f));

                // �������� AI�� ���� ���� ������ Ÿ�ϵ��� NavMesh Build
                var nav_mesh_surface = tile.GetComponent<NavMeshSurface>();
                if (nav_mesh_surface != null)
                {
                    nav_mesh_surface.BuildNavMesh();
                }

                // �ش���ġ�� �� Ȥ�� ���� ������Ʈ�� �������� ���� ����
                GameObject random_object = GetRandomObject();

                // �ش� ��ü�� �������� �����Ұ��
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
