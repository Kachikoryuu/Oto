﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using static DxLibDLL.DX;
using Amaoto;
using TJAParse;
using Tunebeat.Common;

namespace Tunebeat.Game
{
    public class Notes : Scene
    {
        public override void Enable()
        {
            for (int i = 0; i < 2; i++)
            {
                UseSudden[i] = PlayData.Data.UseSudden[i];
                Sudden[i] = PlayData.Data.SuddenNumber[i];
                Scroll[i] = PlayData.Data.ScrollSpeed[i];
                PreGreen[i] = PlayData.Data.GreenNumber[i];
                NHSNumber[i] = PlayData.Data.NHSSpeed[i];
                if (PlayData.Data.NormalHiSpeed[i]) SetNHSScroll(i, true);
                if (PlayData.Data.FloatingHiSpeed[i]) SetScroll(i, true);
            }
            switch ((EPreviewType)PlayData.Data.PreviewType)
            {
                case EPreviewType.Up:
                    NotesP = new Point[2] { new Point(521, 52), new Point(521, 552 - 290 + 52) };
                    break;
                case EPreviewType.Down:
                    NotesP = new Point[2] { new Point(521, Game.Play2P ? 1080 - 461 - 48 : 1080 - 199), new Point(521, 1080 - 199 - 48) };
                    break;
                case EPreviewType.Normal:
                default:
                    NotesP = new Point[2] { new Point(521, 290), new Point(521, 552) };
                    break;
                case EPreviewType.AllCourses:
                    for (int i = 1; i < 5; i++)
                    {
                        Scroll[i] = PlayData.Data.ScrollSpeed[0];
                    }
                    int count = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (Game.MainTJA[i].Courses[Game.Course[i]].ListChip.Count > 0)
                        {
                            if (i > 0 && Game.Course[i] == Game.Course[i - 1]) break;
                            count++;
                        }
                    }
                    switch (count)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            NotesP = new Point[3] { new Point(521, 290), new Point(521, 290 + 199), new Point(521, 290 + 199 * 2) };
                            break;
                        case 4:
                            NotesP = new Point[4] { new Point(521, 85 + 199), new Point(521, 85 + 199 * 2), new Point(521, 85 + 199 * 3), new Point(521, 85 + 199 * 4) };
                            break;
                        case 5:
                            NotesP = new Point[5] { new Point(521, 85), new Point(521, 85 + 199), new Point(521, 85 + 199 * 2), new Point(521, 85 + 199 * 3), new Point(521, 85 + 199 * 4) };
                            break;
                    }
                    break;
            }
            for (int i = 0; i < 5; i++)
            {
                ProcessAuto.RollTimer[i] = new Counter((long)0.0, (long)(1000.0 / PlayData.Data.AutoRoll), (long)1000.0, false);
            }
            base.Enable();
        }

        public override void Disable()
        {
            for (int i = 0; i < 5; i++)
            {
                ProcessAuto.RollTimer[i].Reset();
            }
            base.Disable();
        }

        public override void Draw()
        {
            if ((EPreviewType)PlayData.Data.PreviewType == EPreviewType.AllCourses)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Game.MainTJA[i].Courses[Game.Course[i]].ListChip.Count > 0)
                    {
                        if (i > 0 && Game.Course[i] == Game.Course[i - 1]) break;
                        DrawNotes(i);

                        if ((PlayData.Data.PlayMovie && File.Exists(Game.MainMovie.FileName)) || (PlayData.Data.ShowImage && File.Exists(Game.MainImage.FileName)))
                        {
                            TextureLoad.Game_Base.Opacity = 0.75;
                            TextureLoad.Game_Lane_Frame.Opacity = 0.75;
                            TextureLoad.Game_Gauge_Base.Opacity = 0.75;
                        }
                        else
                        {
                            TextureLoad.Game_Base.Opacity = 1.0;
                            TextureLoad.Game_Lane_Frame.Opacity = 1.0;
                            TextureLoad.Game_Gauge_Base.Opacity = 1.0;
                        }
                        TextureLoad.Game_Base.Color = Color.FromArgb(PlayData.Data.SkinColor[0], PlayData.Data.SkinColor[1], PlayData.Data.SkinColor[2]);
                        TextureLoad.Game_Base.Draw(0, NotesP[i].Y - 4);
                        TextureLoad.Game_Base_Info.Draw(0, NotesP[i].Y - 4);
                        TextureLoad.Game_Lane_Frame.Draw(495, NotesP[i].Y - 4);
                    }
                }
            }
            else
            {
                if (Game.MainTJA[0].Courses[Game.Course[0]].ScrollType == EScroll.Normal)
                {
                    DrawNotes(0);
                }
                else
                {
                    DrawNotes(0);
                    //DrawNotesHBS(0, Game.MainTJA[0].Courses[Game.Course[0]].ScrollType == EScroll.HBSCROLL ? true : false);
                }
                if (Game.Play2P)
                {
                    if (Game.MainTJA[1].Courses[Game.Course[1]].ScrollType == EScroll.Normal)
                    {
                        DrawNotes(1);
                    }
                    else
                    {
                        DrawNotes(1);
                        //DrawNotesHBS(1, Game.MainTJA[1].Courses[Game.Course[1]].ScrollType == EScroll.HBSCROLL ? true : false);
                    }
                }

                TextureLoad.Game_Sudden.Color = Color.FromArgb(PlayData.Data.SkinColor[0], PlayData.Data.SkinColor[1], PlayData.Data.SkinColor[2]);
                if (UseSudden[0])
                {
                    TextureLoad.Game_Sudden.Draw(NotesP[0].X - 22 + (TextureLoad.Game_Lane.TextureSize.Width * (1000 - Sudden[0]) / 1000), NotesP[0].Y);
                }
                if (UseSudden[1] && Game.Play2P)
                {
                    TextureLoad.Game_Sudden.Draw(NotesP[1].X - 22 + (TextureLoad.Game_Lane.TextureSize.Width * (1000 - Sudden[1]) / 1000), NotesP[1].Y);
                }

                if (Key.IsPushing(KEY_INPUT_LSHIFT))
                {
                    int type = 0;
                    if (PlayData.Data.FloatingHiSpeed[0]) type = 1;
                    else if (PlayData.Data.NormalHiSpeed[0]) type = 2;
                    TextureLoad.Game_HiSpeed.Draw(NotesP[0].X - 24, NotesP[0].Y - 2, new Rectangle(0, 56 * type, 195, 56));
                    Score.DrawNumber(PlayData.Data.NormalHiSpeed[0] && !PlayData.Data.FloatingHiSpeed[0] ? NotesP[0].X + 16 : NotesP[0].X - 4, NotesP[0].Y + 18, $"{Scroll[0],6:F2}", 0);
                    if (PlayData.Data.NormalHiSpeed[0] && !PlayData.Data.FloatingHiSpeed[0]) Score.DrawNumber(NotesP[0].X - 16, NotesP[0].Y + 18, $"{NHSNumber[0] + 1,2}", 0);
                    Score.DrawNumber(Sudden[0] < 54 ? 1842 : NotesP[0].X - 22 + (TextureLoad.Game_Lane.TextureSize.Width * (1000 - Sudden[0]) / 1000), 348, $"{Sudden[0]}", 0);
                    Score.DrawNumber(Sudden[0] < 54 ? 1842 : NotesP[0].X - 22 + (TextureLoad.Game_Lane.TextureSize.Width * (1000 - Sudden[0]) / 1000), 388, $"{(GreenNumber[0] > 0 ? GreenNumber[0] : 0)}", 5);
                }
                if (Key.IsPushing(KEY_INPUT_RSHIFT))
                {
                    int type = 0;
                    if (PlayData.Data.FloatingHiSpeed[1]) type = 1;
                    else if (PlayData.Data.NormalHiSpeed[1]) type = 2;
                    TextureLoad.Game_HiSpeed.Draw(NotesP[1].X - 24, NotesP[1].Y - 2, new Rectangle(0, 56 * type, 195, 56));
                    Score.DrawNumber(PlayData.Data.NormalHiSpeed[1] && !PlayData.Data.FloatingHiSpeed[1] ? NotesP[1].X + 16 : NotesP[1].X - 4, NotesP[1].Y + 18, $"{Scroll[1],6:F2}", 0);
                    if (PlayData.Data.NormalHiSpeed[1] && !PlayData.Data.FloatingHiSpeed[1]) Score.DrawNumber(NotesP[1].X - 16, NotesP[1].Y + 18, $"{NHSNumber[1] + 1,2}", 0);
                    Score.DrawNumber(Sudden[1] < 54 ? 1842 : NotesP[1].X - 22 + (TextureLoad.Game_Lane.TextureSize.Width * (1000 - Sudden[1]) / 1000), 616, $"{Sudden[1]}", 0);
                    Score.DrawNumber(Sudden[1] < 54 ? 1842 : NotesP[1].X - 22 + (TextureLoad.Game_Lane.TextureSize.Width * (1000 - Sudden[1]) / 1000), 656, $"{(GreenNumber[1] > 0 ? GreenNumber[1] : 0)}", 5);
                }
                if ((PlayData.Data.PlayMovie && File.Exists(Game.MainMovie.FileName)) || (PlayData.Data.ShowImage && File.Exists(Game.MainImage.FileName)))
                {
                    TextureLoad.Game_Base_DP.Opacity = 0.75;
                    TextureLoad.Game_Base.Opacity = 0.75;
                    TextureLoad.Game_Lane_Frame_DP.Opacity = 0.75;
                    TextureLoad.Game_Lane_Frame.Opacity = 0.75;
                    TextureLoad.Game_Gauge_Base.Opacity = 0.75;
                }
                else
                {
                    TextureLoad.Game_Base_DP.Opacity = 1.0;
                    TextureLoad.Game_Base.Opacity = 1.0;
                    TextureLoad.Game_Lane_Frame_DP.Opacity = 1.0;
                    TextureLoad.Game_Lane_Frame.Opacity = 1.0;
                    TextureLoad.Game_Gauge_Base.Opacity = 1.0;
                }

                if (Game.Play2P)
                {
                    TextureLoad.Game_Base_DP.Color = Color.FromArgb(PlayData.Data.SkinColor[0], PlayData.Data.SkinColor[1], PlayData.Data.SkinColor[2]);
                    TextureLoad.Game_Base_DP.Draw(0, NotesP[0].Y - 4);
                    TextureLoad.Game_Base_Info_DP.Draw(0, NotesP[0].Y - 4);
                    TextureLoad.Game_Lane_Frame_DP.Draw(495, NotesP[0].Y - 4);
                }
                else
                {
                    TextureLoad.Game_Base.Color = Color.FromArgb(PlayData.Data.SkinColor[0], PlayData.Data.SkinColor[1], PlayData.Data.SkinColor[2]);
                    TextureLoad.Game_Base.Draw(0, NotesP[0].Y - 4);
                    TextureLoad.Game_Base_Info.Draw(0, NotesP[0].Y - 4);
                    TextureLoad.Game_Lane_Frame.Draw(495, NotesP[0].Y - 4);
                }
            }

#if DEBUG

            if (UseSudden[0]) DrawString(1700, 360, $"{Sudden[0]}", 0xffffff);
            DrawString(1700, 400, $"{GreenNumber[0]}", 0x00ff00);
            if (PlayData.Data.NormalHiSpeed[0]) DrawString(1600, 380, $"{NHSNumber[0] + 1}", 0x0000ff);
            if (PlayData.Data.FloatingHiSpeed[0]) DrawString(1600, 360, "FHS", 0xffffff);
            if (Game.Play2P)
            {
                if (UseSudden[1]) DrawString(1700, 620, $"{Sudden[1]}", 0xffffff);
                DrawString(1700, 660, $"{GreenNumber[1]}", 0x00ff00);
                if (PlayData.Data.NormalHiSpeed[1]) DrawString(1600, 640, $"{NHSNumber[1] + 1}", 0x0000ff);
                if (PlayData.Data.FloatingHiSpeed[1]) DrawString(1600, 620, "FHS", 0xffffff);
            }
#endif

            base.Draw();
        }

        public override void Update()
        {
            for (int i = 0; i < 2; i++)
            {
                GreenNumber[i] = GetGreenNumber(i);
                Chip chip = GetNotes.GetNowNote(Game.MainTJA[i].Courses[Game.Course[i]].ListChip, Game.MainTimer.Value - Game.Adjust[i]);
                if (chip != null && chip.RollCount == 0 && chip.ENote >= ENote.RollStart && chip.ENote != ENote.RollEnd)
                {
                    ProcessNote.BalloonRemain[i] = ProcessNote.BalloonAmount(i);
                }
                if (chip != null && (chip.ENote == ENote.Balloon || chip.ENote == ENote.Kusudama) && chip.RollEnd != null && chip.RollEnd.Time <= Game.MainTimer.Value - Game.Adjust[i] && ProcessNote.BalloonRemain[i] > 0)
                {
                    ProcessNote.BalloonRemain[i] = 0;
                    ProcessNote.BalloonList[i]++;
                }
                if (PlayData.Data.NormalHiSpeed[i] && !PlayData.Data.FloatingHiSpeed[i]) SetNHSScroll(i, Game.MainTimer.State == 0);
            }

            if (Game.MainTimer.State == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    Scroll[i] = PlayData.Data.ScrollSpeed[i];
                    UseSudden[i] = PlayData.Data.UseSudden[i];
                    Sudden[i] = PlayData.Data.SuddenNumber[i];
                    NHSNumber[i] = PlayData.Data.NHSSpeed[i];
                    if (PlayData.Data.FloatingHiSpeed[i]) PlayData.Data.GreenNumber[i] = PreGreen[i];
                    else PlayData.Data.GreenNumber[i] = GreenNumber[i];
                }
            }

            base.Update();
        }

        public static void DrawNotes(int player)
        {
            if ((PlayData.Data.PlayMovie && File.Exists(Game.MainMovie.FileName)) || (PlayData.Data.ShowImage && File.Exists(Game.MainImage.FileName)))
            {
                TextureLoad.Game_Lane.Opacity = 0.5;
            }
            else
            {
                TextureLoad.Game_Lane.Opacity = 1.0;
            }
            TextureLoad.Game_Lane.Draw(NotesP[player].X - 22, NotesP[player].Y);
            Chip nchip = GetNotes.GetNowNote(Game.MainTJA[player].Courses[Game.Course[player]].ListChip, Game.MainTimer.Value, true);
            if (nchip != null && nchip.IsGogo)
            {
                if ((PlayData.Data.PlayMovie && File.Exists(Game.MainMovie.FileName)) || (PlayData.Data.ShowImage && File.Exists(Game.MainImage.FileName)))
                {
                    TextureLoad.Game_Lane_Gogo.Opacity = 0.25;
                }
                else
                {
                    TextureLoad.Game_Lane_Gogo.Opacity = 0.5;
                }
                TextureLoad.Game_Lane_Gogo.BlendMode = BlendMode.Add;
                TextureLoad.Game_Lane_Gogo.Draw(NotesP[player].X - 22, NotesP[player].Y);
            }

            TextureLoad.Game_Notes.Draw(NotesP[player].X, NotesP[player].Y, new Rectangle(0, 0, 195, 195));

            for (int i = 0; i < Game.MainTJA[player].Courses[Game.Course[player]].ListChip.Count; i++)
            {
                Chip chip = Game.MainTJA[player].Courses[Game.Course[player]].ListChip[i];
                double time = chip.Time - Game.MainTimer.Value;
                float x = (float)NotesX(chip.Time, Game.MainTimer.Value + Game.TimeRemain, chip.Bpm, chip.Scroll, player);
                if (chip.EChip == EChip.Measure && chip.IsShow && x <= 1500 && x >= -715)
                {
                    TextureLoad.Game_Bar.Draw(NotesP[player].X + 96 + x, NotesP[player].Y);
                }

                //オートの処理呼び出し
                ProcessAuto.Update(PlayData.Data.PreviewType == 3 ? true : Game.IsAuto[player], chip, Game.MainTimer.Value, player);
                if (PlayData.Data.PreviewType < 3)
                {
                    ProcessReplay.Update(Game.IsReplay[player], player);
                    ProcessReplay.UnderUpdate();
                }
                //ノーツが通り過ぎた時の処理
                ProcessNote.PassNote(chip, time, chip.ENote == ENote.Ka || chip.ENote == ENote.KA ? false : true, player);
            }
            //連打のタイマー　なんでここ？？
            Chip nowchip = GetNotes.GetNowNote(Game.MainTJA[player].Courses[Game.Course[player]].ListChip, Game.MainTimer.Value);
            ERoll roll = nowchip != null ? ProcessNote.RollState(nowchip) : ERoll.None;
            for (int i = 0; i < 5; i++)
            {
                ProcessAuto.RollTimer[i].Tick();
            }
            if (roll != ERoll.None)
            {
                ProcessAuto.RollTimer[player].Start();
            }
            else
            {
                ProcessAuto.RollTimer[player].Reset();
            }

            for (int i = Game.MainTJA[player].Courses[Game.Course[player]].ListChip.Count - 1; i >= 0; i--)
            {
                Chip chip = Game.MainTJA[player].Courses[Game.Course[player]].ListChip[i];
                float x = (float)NotesX(chip.Time, Game.MainTimer.Value + Game.TimeRemain, chip.Bpm, chip.Scroll, player);

                if (chip.EChip == EChip.Note && x <= 1500 && !chip.IsHit && (PlayData.Data.PreviewType == 3 || (player < 2 && !PlayData.Data.Stelth[player])))
                {
                    switch (chip.ENote)
                    {
                        case ENote.Don:
                        case ENote.Ka:
                        case ENote.DON:
                        case ENote.KA:
                            if (x >= -715)
                                TextureLoad.Game_Notes.Draw(NotesP[player].X + x, NotesP[player].Y, new Rectangle((int)chip.ENote * 195, 0, 195, 195));
                            break;
                        case ENote.RollStart:
                        case ENote.ROLLStart:
                            double rollx = NotesX(chip.RollEnd != null ? chip.RollEnd.Time : chip.Time, Game.MainTimer.Value + Game.TimeRemain, chip.Bpm, chip.RollEnd != null ? chip.RollEnd.Scroll : chip.Scroll, player);
                            if (rollx >= -715)
                            {
                                TextureLoad.Game_Notes.ScaleX = (float)(rollx - x);
                                TextureLoad.Game_Notes.Draw(NotesP[player].X + x - 3 + 112, NotesP[player].Y + 1, new Rectangle(195 * (chip.ENote == ENote.RollStart ? 6 : 9) + 10, 0, 1, 195)); //連打の中身
                                TextureLoad.Game_Notes.ScaleX = 1f;
                                TextureLoad.Game_Notes.Draw(NotesP[player].X + rollx - 4.5f + 112, NotesP[player].Y + 1, new Rectangle(195 * (chip.ENote == ENote.RollStart ? 7 : 10), 0, 195, 195)); //連打の末端の顔
                                TextureLoad.Game_Notes.Draw(NotesP[player].X + x, NotesP[player].Y, new Rectangle(195 * (chip.ENote == ENote.RollStart ? 5 : 8), 0, 195, 195)); //連打の先頭の顔
                            }
                            break;
                        case ENote.Balloon:
                        case ENote.Kusudama:
                            double ballx = NotesX(chip.RollEnd != null ? chip.RollEnd.Time : chip.Time, Game.MainTimer.Value + Game.TimeRemain, chip.Bpm, chip.Scroll, player);
                            if (ballx >= -715)
                            {
                                if (x > 0)
                                    TextureLoad.Game_Notes.Draw(NotesP[player].X + x, NotesP[player].Y, new Rectangle(195 * (chip.ENote == ENote.Balloon ? 11 : 13), 0, 390, 195));
                                else if (ballx > 0)
                                    TextureLoad.Game_Notes.Draw(NotesP[player].X, NotesP[player].Y, new Rectangle(195 * (chip.ENote == ENote.Balloon ? 11 : 13), 0, 390, 195));
                                else
                                {
                                    TextureLoad.Game_Notes.Draw(NotesP[player].X + ballx, NotesP[player].Y, new Rectangle(195 * (chip.ENote == ENote.Balloon ? 11 : 13), 0, 390, 195));
                                }
                            }
                            break;
                    }
                }
            }

        }

        public static double NotesX(double chiptime, double timer, double bpm,  double scroll, int player)
        {
            double time = chiptime - timer;
            double x = time * bpm * scroll * 2.0 * (Scroll[player] - Game.ScrollRemain[player]) / 335.2396;
            return x;
        }

        public static void DrawNotesHBS(int player, bool isHBS)
        {
            TextureLoad.Game_Lane.Draw(NotesP[player].X - 22, NotesP[player].Y);

            TextureLoad.Game_Notes.Draw(NotesP[player].X, NotesP[player].Y, new Rectangle(0, 0, 195, 195));

            for (int i = 0; i < Game.MainTJA[player].Courses[Game.Course[player]].ListChip.Count; i++)
            {
                Chip chip = Game.MainTJA[player].Courses[Game.Course[player]].ListChip[i];
                double time = chip.Time - Game.MainTimer.Value;
                //float x = (float)NotesX(chip.Time, Game.MainTimer.Value, chip.Bpm, chip.Scroll);
                //if (chip.EChip == EChip.Measure && chip.IsShow && x <= 1500 && x >= -715)
                //{
                //    TextureLoad.Game_Bar.Draw(NotesP[player].X + 96 + x, NotesP[player].Y);
                //}

                //オートの処理呼び出し
                ProcessAuto.Update(PlayData.Data.PreviewType == 3 || Game.IsAuto[player], chip, Game.MainTimer.Value, player);
                //ノーツが通り過ぎた時の処理
                ProcessNote.PassNote(chip, time, chip.ENote == ENote.Ka || chip.ENote == ENote.KA ? false : true, player);
            }
            //連打のタイマー　なんでここ？？
            Chip nowchip = GetNotes.GetNowNote(Game.MainTJA[player].Courses[Game.Course[player]].ListChip, Game.MainTimer.Value);
            ERoll roll = nowchip != null ? ProcessNote.RollState(nowchip) : ERoll.None;
            for (int i = 0; i < 5; i++)
            {
                ProcessAuto.RollTimer[i].Tick();
            }
            for (int i = 0; i < 5; i++)
            {
                if (roll != ERoll.None)
                {
                    ProcessAuto.RollTimer[i].Start();
                }
                else
                {
                    ProcessAuto.RollTimer[i].Reset();
                }
            }
        }

        public static int GetGreenNumber(int player, double plusminus = 0)
        {
            Chip chip = GetNotes.GetNowNote(Game.MainTJA[player].Courses[Game.Course[player]].ListChip, Game.MainTimer.Value);
            if (chip == null) chip = GetNotes.GetNearNote(Game.MainTJA[player].Courses[Game.Course[player]].ListChip, Game.MainTimer.Value);
            double bpm = chip != null ? chip.Bpm : Game.MainTJA[player].Header.BPM;
            double scroll = chip != null ? chip.Scroll * (Scroll[player] + plusminus) : (Scroll[player] + plusminus);
            int ms = scroll > 0 ? Showms[0] : Showms[1];
            int sudden = UseSudden[player] ? Sudden[player] : 0;
            double suddenrate = 1000.0 / (1000 - sudden);
            return (int)(ms / (bpm * scroll * 2.0 * suddenrate));
        }

        public static void SetNHSScroll(int player, bool isLoad = false)
        {
            Chip chip = new Chip();
            for (int i = 0; i < Game.MainTJA[player].Courses[Game.Course[player]].ListChip.Count; i++)
            {
                if (Game.MainTJA[player].Courses[Game.Course[player]].ListChip[i].ENote >= ENote.Don)
                {
                    chip = Game.MainTJA[player].Courses[Game.Course[player]].ListChip[i];
                    break;
                }
            }
            double bpm = chip != null ? chip.Bpm : Game.MainTJA[player].Header.BPM;
            double scroll = chip != null ? chip.Scroll : 1.0;
            double prescroll = Scroll[player];
            Scroll[player] = Math.Round(Showms[0] / (NHSTargetGNum[NHSNumber[player]] * bpm * scroll * 2), 2, MidpointRounding.AwayFromZero);
            Game.ScrollRemain[player] += Scroll[player] - prescroll;

            if (isLoad) PlayData.Data.ScrollSpeed[player] = Scroll[player];
        }

        public static void SetScroll(int player, bool isLoad = false)
        {
            Chip chip = GetNotes.GetNowNote(Game.MainTJA[player].Courses[Game.Course[player]].ListChip, Game.MainTimer.Value);
            if (chip == null) chip = GetNotes.GetNearNote(Game.MainTJA[player].Courses[Game.Course[player]].ListChip, Game.MainTimer.Value);
            double bpm = chip != null ? chip.Bpm : Game.MainTJA[player].Header.BPM;
            double scroll = chip != null ? chip.Scroll : 1.0;
            int sudden = UseSudden[player] ? Sudden[player] : 0;
            double suddenrate = 1000.0 / (1000 - sudden);
            double prescroll = Scroll[player];
            Scroll[player] = Math.Round(Showms[0] / (PreGreen[player] * bpm * scroll * 2.0 * suddenrate), 2, MidpointRounding.AwayFromZero);
            Game.ScrollRemain[player] += Scroll[player] - prescroll;

            if (isLoad) PlayData.Data.ScrollSpeed[player] = Scroll[player];
        }

        public static void SetSudden(int player, bool isPlus, bool isLoad = false, bool Reset = false)
        {
            if (!isLoad)
            {
                if (Reset)
                {
                    Sudden[player] = PlayData.Data.SuddenNumber[player];
                }
                else
                {
                    if (isPlus)
                        Sudden[player] += 2;
                    else Sudden[player] -= 2;
                }
            }
            else
            {
                if (isPlus)
                    PlayData.Data.SuddenNumber[player] += 2;
                else PlayData.Data.SuddenNumber[player] -= 2;
            }
            
            if (PlayData.Data.FloatingHiSpeed[player]) SetScroll(player, isLoad);
        }

        public static Point[] NotesP = new Point[5];
        public static int[] Showms = new int[2] { 256300, -37000 };
        public static double[] Scroll = new double[5];
        public static bool[] UseSudden = new bool[2];
        public static int[] Sudden = new int[2], GreenNumber = new int[2], PreGreen = new int[2], NHSNumber = new int[2];
        public static int[] NHSTargetGNum = new int[20] { 1200, 1000, 800, 700, 650, 600, 550, 500, 480, 460,
            440, 420, 400, 380, 360, 340, 320, 300, 280, 260 };
    }

    public enum EPreviewType
    {
        Normal,
        Up,
        Down,
        AllCourses
    }
}
