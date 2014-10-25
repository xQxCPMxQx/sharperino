using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace D_Nidalee
{
    class Program
    {
        public const string ChampionName = "Nidalee";

        public static Orbwalking.Orbwalker Orbwalker;

        public static Spell Q, W, E, R, QC, WC, EC;

        public static List<Spell> SpellList = new List<Spell>();

        public static Items.Item HDR, BKR, BWC, YOU, DFG, SOD, FQM;

        public static SpellSlot IgniteSlot;

        public static Menu Config;

        private static Obj_AI_Hero Player;

        public static bool IsHuman;

        public static bool IsCougar;

        public static bool Recall;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            Player = ObjectManager.Player;
            if (ObjectManager.Player.BaseSkinName != ChampionName) return;



            Q = new Spell(SpellSlot.Q, 1500f);
            W = new Spell(SpellSlot.W, 900f);
            E = new Spell(SpellSlot.E, 600f);
            WC = new Spell(SpellSlot.W, 750f);
            EC = new Spell(SpellSlot.E, 300f);
            R = new Spell(SpellSlot.R, 0);

            Q.SetSkillshot(0.125f, 70f, 1300, true, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.500f, 80f, 1450, false, SkillshotType.SkillshotCircle);

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);
            SpellList.Add(QC);
            SpellList.Add(WC);
            SpellList.Add(EC);

            HDR = new Items.Item(3074, 175f);
            BKR = new Items.Item(3153, 450f);
            BWC = new Items.Item(3144, 450f);
            YOU = new Items.Item(3142, 185f);
            DFG = new Items.Item(3128, 750f);
            FQM = new Items.Item(3092, 850f);

            IgniteSlot = Player.GetSpellSlot("SummonerDot");

            //D Nidalee
            Config = new Menu("D - Nidalee", "D-Nidalee", true);

            //TargetSelector
            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(targetSelectorMenu);
            Config.AddSubMenu(targetSelectorMenu);

            //Orbwalker
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            //Combo
            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "Use R")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQComboCougar", "Use Q Cougar")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseWComboCougar", "Use W Cougar")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseEComboCougar", "Use E Cougar")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseItems", "Use Items")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("ActiveCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            //Extra
            Config.AddSubMenu(new Menu("Extra", "Extra"));
            Config.SubMenu("Extra").AddItem(new MenuItem("UseAutoE", "Use auto E")).SetValue(true);
            Config.SubMenu("Extra").AddItem(new MenuItem("HPercent", "Health percent")).SetValue(new Slider(40, 1, 100));
            Config.SubMenu("Extra").AddItem(new MenuItem("AllyUseAutoE", "Ally Use auto E")).SetValue(true);
            Config.SubMenu("Extra").AddItem(new MenuItem("AllyHPercent", "Health percent")).SetValue(new Slider(40, 1, 100));



            //Harass
            Config.AddSubMenu(new Menu("Harass", "Harass"));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseQHarass", "Use Q")).SetValue(true);
            Config.SubMenu("Harass").AddItem(new MenuItem("UseWHarass", "Use W")).SetValue(true);
            Config.SubMenu("Harass").AddItem(new MenuItem("UseEHarass", "Use E")).SetValue(true);
            Config.SubMenu("Harass").AddItem(new MenuItem("ActiveHarass", "Harass key").SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("Harass").AddItem(new MenuItem("Harrasmana", "Minimum Mana").SetValue(new Slider(60, 1, 100)));

            //Farm
            Config.AddSubMenu(new Menu("Lane Clear", "Lane"));
            Config.SubMenu("Lane").AddItem(new MenuItem("farm_E1", "Use E (Human)")).SetValue(true);
            Config.SubMenu("Lane").AddItem(new MenuItem("UseQLane", "Use Q (Cougar)")).SetValue(true);
            Config.SubMenu("Lane").AddItem(new MenuItem("UseWLane", "Use W (Cougar)")).SetValue(true);
            Config.SubMenu("Lane").AddItem(new MenuItem("UseELane", "Use E (Cougar)")).SetValue(true);
            Config.SubMenu("Lane").AddItem(new MenuItem("LaneClear", "Clear key").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            Config.SubMenu("Lane").AddItem(new MenuItem("farm_R", "Auto Switch Forms(toggle)").SetValue(new KeyBind("G".ToCharArray()[0], KeyBindType.Toggle)));
            Config.SubMenu("Lane").AddItem(new MenuItem("Lane", "Minimum Mana").SetValue(new Slider(60, 1, 100)));
            //Kill Steal
            Config.AddSubMenu(new Menu("KillSteal", "Ks"));
            Config.SubMenu("Ks").AddItem(new MenuItem("ActiveKs", "Use KillSteal")).SetValue(true);
            Config.SubMenu("Ks").AddItem(new MenuItem("UseQKs", "Use Q")).SetValue(true);
            Config.SubMenu("Ks").AddItem(new MenuItem("UseIgnite", "Use Ignite")).SetValue(true);


            //Drawings
            Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawW", "Draw W")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("DrawE", "Draw E")).SetValue(true);
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleLag", "Lag Free Circles").SetValue(true));
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleQuality", "Circles Quality").SetValue(new Slider(100, 100, 10)));
            Config.SubMenu("Drawings").AddItem(new MenuItem("CircleThickness", "Circles Thickness").SetValue(new Slider(1, 10, 1)));

            Config.AddToMainMenu();

            Game.OnGameUpdate += Game_OnGameUpdate;
            Obj_AI_Hero.OnCreate += OnCreateObj;
            Obj_AI_Hero.OnDelete += OnDeleteObj;
            Drawing.OnDraw += Drawing_OnDraw;
            Game.PrintChat("<font color='#881df2'>Nidallee By Diabaths </font>Loaded!");
        }



        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Config.Item("UseAutoE").GetValue<bool>())
            {
                AutoE();
            }
            AllyAutoE();
            Player = ObjectManager.Player;
            QC = new Spell(SpellSlot.Q, Player.AttackRange + 50);
            SOD = new Items.Item(3131, Player.AttackRange + 50);
            Orbwalker.SetAttack(true);

            CheckSpells();

            if (Config.Item("ActiveCombo").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            if (Config.Item("ActiveHarass").GetValue<KeyBind>().Active && Config.Item("UseQHarass").GetValue<bool>() && Q.IsReady() && (100 * (Player.Mana / Player.MaxMana)) > Config.Item("Harrasmana").GetValue<Slider>().Value)
            {
                Harass();
            }
            if (Config.Item("LaneClear").GetValue<KeyBind>().Active)
            {
                Farm();
            }
            if (Config.Item("ActiveKs").GetValue<bool>())
            {
                KillSteal();
            }
        }

        private static void Combo()
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            Orbwalker.SetAttack((!Q.IsReady() || W.IsReady()));

            if (target != null)
            {


                if (IsHuman && Player.Distance(target) <= Q.Range && Config.Item("UseQCombo").GetValue<bool>() && Q.IsReady())
                {
                    Q.Cast(target, false);

                }
                if (IsHuman && Player.Distance(target) <= W.Range && Config.Item("UseWCombo").GetValue<bool>() && W.IsReady())
                {
                    W.Cast(target);
                }

                if (IsHuman && Config.Item("UseRCombo").GetValue<bool>() && Player.Distance(target) <= 625 && R.IsReady())
                {
                    if (IsHuman) { R.Cast(); }

                    if (IsCougar)
                    {
                        if (Config.Item("UseWComboCougar").GetValue<bool>() && Player.Distance(target) <= WC.Range)
                        {
                            WC.Cast(target);
                        }
                        if (Config.Item("UseEComboCougar").GetValue<bool>() && Player.Distance(target) <= EC.Range)
                        {
                            EC.Cast(target);
                        }
                        if (Config.Item("UseQComboCougar").GetValue<bool>() && Player.Distance(target) <= Q.Range)
                        {
                            Orbwalker.SetAttack(true);
                            QC.Cast(target);
                        }

                    }

                }

                if (IsCougar && Player.Distance(target) < 625)
                {
                    if (IsHuman) { R.Cast(); }

                    if (IsCougar)
                    {
                        if (Config.Item("UseWComboCougar").GetValue<bool>() && Player.Distance(target) <= WC.Range)
                        {
                            WC.Cast(target);
                        }
                        if (Config.Item("UseEComboCougar").GetValue<bool>() && Player.Distance(target) <= EC.Range)
                        {
                            EC.Cast(target);
                        }
                        if (Config.Item("UseQComboCougar").GetValue<bool>() && Player.Distance(target) <= Q.Range)
                        {
                            Orbwalker.SetAttack(true);
                            QC.Cast(target);
                        }

                    }
                }

                if (IsCougar && Config.Item("UseRCombo").GetValue<bool>() && Player.Distance(target) > WC.Range)
                {
                    R.Cast();
                }
                if (IsCougar && Player.Distance(target) > EC.Range && Config.Item("UseRCombo").GetValue<bool>())
                {
                    R.Cast();
                }

                if (Config.Item("UseItems").GetValue<bool>())
                {
                    BKR.Cast(target);
                    YOU.Cast();
                    BWC.Cast(target);
                    DFG.Cast(target);
                    SOD.Cast();
                    FQM.Cast(target);
                }
            }
        }


        private static void Harass()
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            if (target != null)
            {

                if (IsHuman && Player.Distance(target) <= Q.Range && Config.Item("UseQHarass").GetValue<bool>() && Q.IsReady() && Q.GetPrediction(target).Hitchance >= HitChance.High)
                {
                    Q.Cast(target);
                }

                if (IsHuman && Player.Distance(target) <= W.Range && Config.Item("UseWHarass").GetValue<bool>() && W.IsReady())
                {
                    W.Cast(target);
                }

                if (IsHuman && E.IsReady() && Config.Item("UseEHarass").GetValue<bool>())
                    E.CastOnUnit(Player);
            }
        }

        private static void Farm()
        {
            foreach (Obj_AI_Minion Minion in ObjectManager.Get<Obj_AI_Minion>().Where(minion => minion.Team != Player.Team && !minion.IsDead && Vector2.Distance(minion.ServerPosition.To2D(), Player.ServerPosition.To2D()) < 600f).OrderBy(minion => Vector2.Distance(minion.Position.To2D(), Player.Position.To2D())))
            {
                if (IsCougar)
                {
                    if (QC.IsReady() && Config.Item("UseQLane").GetValue<bool>())
                        QC.Cast(Minion);
                    else if (WC.IsReady() && Config.Item("UseWLane").GetValue<bool>() && Player.Distance(Minion) > 200f)
                        WC.Cast(Minion);
                    else if (EC.IsReady() && Config.Item("UseELane").GetValue<bool>())
                        EC.Cast(Minion);
                }

                else if (R.IsReady() && Config.Item("farm_R").GetValue<KeyBind>().Active)
                    R.Cast();
                else if (E.IsReady() && Config.Item("farm_E1").GetValue<bool>() && (100 * (Player.Mana / Player.MaxMana)) > Config.Item("Lane").GetValue<Slider>().Value)
                    E.CastOnUnit(Player);
                return;
            }
        }

        private static void AutoE()
        {
            if (Player.Spellbook.CanUseSpell(SpellSlot.E) == SpellState.Ready && Player.IsMe)
            {

                if (Player.HasBuff("Recall")) return;

                if (Player.Health <= (Player.MaxHealth * (Config.Item("HPercent").GetValue<Slider>().Value) / 100))
                {
                    Player.Spellbook.CastSpell(SpellSlot.E, Player);
                }

            }


        }

        private static void AllyAutoE()
        {
            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsAlly && !hero.IsMe))
            {
                if (Player.HasBuff("Recall") || hero.HasBuff("Recall")) return;
                if (Config.Item("AllyUseAutoE").GetValue<bool>() && (hero.Health / hero.MaxHealth) * 100 <= Config.Item("AllyHPercent").GetValue<Slider>().Value && E.IsReady() && Utility.CountEnemysInRange(1200) > 0 && hero.Distance(Player.ServerPosition) <= E.Range)
                {
                    E.Cast(hero);
                }
            }
        }

        private static void KillSteal()
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            var igniteDmg = Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);
            var QHDmg = Player.GetSpellDamage(target, SpellSlot.Q);

            if (target != null && Config.Item("UseIgnite").GetValue<bool>() && IgniteSlot != SpellSlot.Unknown &&
            Player.SummonerSpellbook.CanUseSpell(IgniteSlot) == SpellState.Ready)
            {
                if (igniteDmg > target.Health)
                {
                    Player.SummonerSpellbook.CastSpell(IgniteSlot, target);
                }
            }

            if (Q.IsReady() && Player.Distance(target) <= Q.Range && target != null && Config.Item("UseQKs").GetValue<bool>())
            {
                if (target.Health <= QHDmg)
                {
                    Q.Cast(target);
                }
            }
        }

        private static void CheckSpells()
        {
            if (Player.Spellbook.GetSpell(SpellSlot.Q).Name == "JavelinToss" ||
                Player.Spellbook.GetSpell(SpellSlot.W).Name == "Bushwhack" ||
                Player.Spellbook.GetSpell(SpellSlot.E).Name == "PrimalSurge")
            {
                IsHuman = true;
                IsCougar = false;
            }
            if (Player.Spellbook.GetSpell(SpellSlot.Q).Name == "Takedown" ||
                Player.Spellbook.GetSpell(SpellSlot.W).Name == "Pounce" ||
                Player.Spellbook.GetSpell(SpellSlot.E).Name == "Swipe")
            {
                IsHuman = false;
                IsCougar = true;
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Config.Item("CircleLag").GetValue<bool>())
            {
                if (Config.Item("DrawQ").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, Q.Range, System.Drawing.Color.White,
                        Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("DrawW").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, W.Range, System.Drawing.Color.White,
                        Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
                if (Config.Item("DrawE").GetValue<bool>())
                {
                    Utility.DrawCircle(ObjectManager.Player.Position, E.Range, System.Drawing.Color.White,
                        Config.Item("CircleThickness").GetValue<Slider>().Value,
                        Config.Item("CircleQuality").GetValue<Slider>().Value);
                }
            }
            else
            {
                if (Config.Item("DrawQ").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, Q.Range, System.Drawing.Color.White);
                }
                if (Config.Item("DrawW").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, W.Range, System.Drawing.Color.White);
                }
                if (Config.Item("DrawE").GetValue<bool>())
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, E.Range, System.Drawing.Color.White);
                }

            }
        }

        private static void OnCreateObj(GameObject sender, EventArgs args)
        {
            //Recall
            if (!(sender is Obj_GeneralParticleEmmiter)) return;
            var obj = (Obj_GeneralParticleEmmiter)sender;
            if (obj != null && obj.IsMe && obj.Name == "TeleportHome")
            {
                Recall = true;
            }

        }

        private static void OnDeleteObj(GameObject sender, EventArgs args)
        {
            //Recall
            if (!(sender is Obj_GeneralParticleEmmiter)) return;
            var obj = (Obj_GeneralParticleEmmiter)sender;
            if (obj != null && obj.IsMe && obj.Name == "TeleportHome")
            {
                Recall = false;
            }

        }
    }

}