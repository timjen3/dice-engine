﻿using System;
using System.Windows.Forms;
using TTRPG.Engine.Demo2.Engine;

namespace TTRPG.Engine.Demo2
{
	public partial class CombatDemoForm : Form
	{
		CombatDemoService _demo;

		private void WriteMessage(string message) => txtBox_MessageLog.Text += $"{message}\r\n";

		private void EndGame()
		{
			WriteMessage("Game over.");
			btn_Attack.Enabled = false;
			btn_UsePotion.Enabled = false;
			btn_NewGame.Visible = true;
		}

		public CombatDemoForm()
		{
			_demo = new CombatDemoService(WriteMessage);
			InitializeComponent();
			UpdateState();
		}

		private void UpdateState()
		{
			txt_ComputerHP.Text = $"{_demo.Computer.Attributes["HP"]} / {_demo.Computer.Attributes["MAX_HP"]}";
			txt_ComputerPotions.Text = $"{_demo.Computer.Attributes["Potions"]}";
			txt_PlayerHP.Text = $"{_demo.Player.Attributes["HP"]} / {_demo.Player.Attributes["MAX_HP"]}";
			txt_PlayerPotions.Text = $"{_demo.Player.Attributes["Potions"]}";
			btn_Attack.Enabled = _demo.CheckPlayerAttack();
			btn_UsePotion.Enabled = _demo.CheckPlayerUsePotion();
		}

		private void btn_Attack_Click(object sender, EventArgs e)
		{
			txtBox_MessageLog.Clear();
			_demo.PlayerAttack();
			UpdateState();
			if (_demo.IsGameOver()) EndGame();
		}

		private void btn_UsePotion_Click(object sender, EventArgs e)
		{
			txtBox_MessageLog.Clear();
			_demo.PlayerUsePotion();
			UpdateState();
			if (_demo.IsGameOver()) EndGame();
		}

		private void btn_NewGame_Click(object sender, EventArgs e)
		{
			txtBox_MessageLog.Clear();
			_demo = new CombatDemoService(WriteMessage);
			btn_NewGame.Visible = false;
			UpdateState();
		}
	}
}