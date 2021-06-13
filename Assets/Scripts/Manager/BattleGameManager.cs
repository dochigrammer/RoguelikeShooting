using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGameMnager : Singleton<BattleGameMnager>
{
    protected List<PlayerCharaComponent> _PlayerCharas;

    public override void OnInitialize()
    {
        ScoreManager.Instance.Clear();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        _PlayerCharas = new List<PlayerCharaComponent>();

        foreach (var player in players)
        {
            var player_chara_component = player.GetComponent<PlayerCharaComponent>();

            if (player_chara_component != null)
            {
                _PlayerCharas.Add(player_chara_component);
            }
        }
    }

    public List<PlayerCharaComponent>.Enumerator GetPlayerCharas() { return _PlayerCharas.GetEnumerator(); }
}
