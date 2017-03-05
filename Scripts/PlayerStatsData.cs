using System;
using UnityEngine;

// class to make data avaliable for serialization
[Serializable]
class PlayerStatsData
{
	public string playerName;
	public int expirience;
	public int kills;
	public int deads;
	private DateTime lastSaveTime;

	public PlayerStatsData (string playerName, int expirience, int kills, int deads)
	{
		this.playerName = playerName;
		this.expirience = expirience;
		this.kills = kills;
		this.deads = deads;
		this.lastSaveTime = DateTime.UtcNow;
	}

	public DateTime LastSaveTime {
		get {
			return lastSaveTime;
		}
	}

	public string PlayerName {
		get {
			return playerName;
		}
		set {
			playerName = value;
		}
	}

	public int Deads {
		get {
			return deads;
		}
		set {
			deads = value;
		}
	}

	public int Expirience {
		get {
			return expirience;
		}
		set {
			expirience = value;
		}
	}

	public int Kills {
		get {
			return kills;
		}
		set {
			kills = value;
		}
	}
}

