﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using static DxLibDLL.DX;
using Amaoto;
using TJAParse;
using Tunebeat.Common;
using Tunebeat.Game;

namespace Tunebeat.SongSelect
{
    public class SongSelect : Scene
    {
        public override void Enable()
        {
            Alart = new Counter(0, 1000, 1000, false);
            SongLoad.Init();
            if (SongLoad.SongData.Count > 0) NowTJA = SongLoad.SongData[NowSongNumber];
            Title = new Texture();
            SubTitle = new Texture();
            Genre = new Texture();
            BPM = new Texture();
            if (PlayData.Data.FontRendering) FontLoad();
            for (int i = 0; i < 2; i++)
            {
                PushedTimer[i] = new Counter(0, 199, 1000, false);
                PushingTimer[i] = new Counter(0, 49, 1000, false);
            }
            SoundLoad.Don[0].Pan = 0;
            if (PlayData.Data.PlaySpeed != 1.0 && PlayData.Data.ChangeSESpeed) SoundLoad.Don[0].PlaySpeed = PlayData.Data.PlaySpeed;
            SoundLoad.Ka[0].Pan = 0;
            if (PlayData.Data.PlaySpeed != 1.0 && PlayData.Data.ChangeSESpeed) SoundLoad.Ka[0].PlaySpeed = PlayData.Data.PlaySpeed;
            base.Enable();
        }

        public override void Disable()
        {
            NowTJA = null;
            Title = null;
            SubTitle = null;
            Genre = null;
            BPM = null;
            Alart.Reset();
            SongLoad.Dispose();

            for (int i = 0; i < 2; i++)
            {
                PushedTimer[i].Reset();
                PushingTimer[i].Reset();
            }
            base.Disable();
        }
        public override void Draw()
        {
            DrawBox(0, 0, 1919, 1079, GetColor(PlayData.Data.SkinColor[0], PlayData.Data.SkinColor[1], PlayData.Data.SkinColor[2]), TRUE);
            TextureLoad.SongSelect_Background.Draw(0, 0);

            if (NowTJA != null)
            {
                switch (NowTJA.Type)
                {
                    case EType.Score:
                        TextureLoad.SongSelect_Bar_Color.Color = NowTJA.BackColor;
                        TextureLoad.SongSelect_Bar_Color.Opacity = 0.75;
                        TextureLoad.SongSelect_Bar_Color.Draw(1180, -90 + 60 * 10);
                        TextureLoad.SongSelect_Bar.Draw(1180, -90 + 60 * 10);
                        DrawString(1300, -90 + 60 * 10 + 22, NowTJA.Header.TITLE, (uint)ColorTranslator.ToWin32(NowTJA.FontColor));
                        if (NowTJA.Course[PlayData.Data.PlayCourse[0]].IsEnable) Score.DrawNumber(1242 - 12 * Score.Digit(NowTJA.Course[PlayData.Data.PlayCourse[0]].LEVEL), -90 + 60 * 10 + 16, $"{NowTJA.Course[PlayData.Data.PlayCourse[0]].LEVEL}", 0);
                        break;
                    default:
                        TextureLoad.SongSelect_Bar_Folder_Color.Color = NowTJA.BackColor;
                        TextureLoad.SongSelect_Bar_Folder_Color.Draw(1180, -90 + 60 * 10);
                        TextureLoad.SongSelect_Bar_Folder.Draw(1180, -90 + 60 * 10);
                        DrawString(1300 - 60, -90 + 60 * 10 + 22, NowTJA.Title, (uint)ColorTranslator.ToWin32(NowTJA.FontColor));
                        break;
                }

                var prev = NowTJA;
                for (int i = 9; i >= 0; i--)
                {
                    prev = prev.Prev;
                    switch (prev.Type)
                    {
                        case EType.Score:
                            TextureLoad.SongSelect_Bar_Color.Color = prev.BackColor;
                            TextureLoad.SongSelect_Bar_Color.Opacity = 0.75;
                            TextureLoad.SongSelect_Bar_Color.Draw(1212, -90 + 60 * i);
                            TextureLoad.SongSelect_Bar.Draw(1212, -90 + 60 * i);
                            DrawString(1332, -90 + 60 * i + 22, prev.Header.TITLE, (uint)ColorTranslator.ToWin32(prev.FontColor));
                            if (prev.Course[PlayData.Data.PlayCourse[0]].IsEnable) Score.DrawNumber(1242 + 32 - 12 * Score.Digit(prev.Course[PlayData.Data.PlayCourse[0]].LEVEL), -90 + 60 * i + 16, $"{prev.Course[PlayData.Data.PlayCourse[0]].LEVEL}", 0);
                            break;
                        default:
                            TextureLoad.SongSelect_Bar_Folder_Color.Color = prev.BackColor;
                            TextureLoad.SongSelect_Bar_Folder_Color.Draw(1212, -90 + 60 * i);
                            TextureLoad.SongSelect_Bar_Folder.Draw(1212, -90 + 60 * i);
                            DrawString(1332 - 60, -90 + 60 * i + 22, prev.Title, (uint)ColorTranslator.ToWin32(prev.FontColor));
                            break;
                    }

                }
                var next = NowTJA;
                for (int i = 11; i < 21; i++)
                {
                    next = next.Next;
                    switch (next.Type)
                    {
                        case EType.Score:
                            TextureLoad.SongSelect_Bar_Color.Color = next.BackColor;
                            TextureLoad.SongSelect_Bar_Color.Opacity = 0.75;
                            TextureLoad.SongSelect_Bar_Color.Draw(1212, -90 + 60 * i);
                            TextureLoad.SongSelect_Bar.Draw(1212, -90 + 60 * i);
                            DrawString(1332, -90 + 60 * i + 22, next.Header.TITLE, (uint)ColorTranslator.ToWin32(next.FontColor));
                            if (next.Course[PlayData.Data.PlayCourse[0]].IsEnable) Score.DrawNumber(1242 + 32 - 12 * Score.Digit(next.Course[PlayData.Data.PlayCourse[0]].LEVEL), -90 + 60 * i + 16, $"{next.Course[PlayData.Data.PlayCourse[0]].LEVEL}", 0);
                            break;
                        default:
                            TextureLoad.SongSelect_Bar_Folder_Color.Color = next.BackColor;
                            TextureLoad.SongSelect_Bar_Folder_Color.Draw(1212, -90 + 60 * i);
                            TextureLoad.SongSelect_Bar_Folder.Draw(1212, -90 + 60 * i);
                            DrawString(1332 - 60, -90 + 60 * i + 22, next.Title, (uint)ColorTranslator.ToWin32(next.FontColor));
                            break;
                    }
                }
                double len = (Mouse.Y - 540.0) / TextureLoad.SongSelect_Bar.ActualSize.Height;
                int cur = (int)Math.Round(len, 0, MidpointRounding.AwayFromZero);
                if (Mouse.X >= 1212 || (cur == 0 && Mouse.X >= 1180))
                {
                    TextureLoad.SongSelect_Bar_Cursor.Draw(cur == 0 ? 1180 : 1212, -90 + 60 * (10 + cur));
                }

                int[] difXY = new int[2] { 72, 500 - 60 };
                TextureLoad.SongSelect_Difficulty_Base.Draw(difXY[0], difXY[1]);
                if (PlayData.Data.IsPlay2P)
                {
                    TextureLoad.SongSelect_Difficulty_Base.Draw(difXY[0], difXY[1] + 163, new Rectangle(0, 132, 814, 31));
                }

                switch (NowTJA.Type)
                {
                    case EType.Score:
                        if (PlayData.Data.FontRendering)
                        {
                            Genre.ScaleX = Genre.TextureSize.Width > 814.0 ? (814.0 / Genre.TextureSize.Width) : 1.0;
                            Title.ScaleX = Title.TextureSize.Width > 814.0 ? (814.0 / Title.TextureSize.Width) : 1.0;
                            SubTitle.ScaleX = SubTitle.TextureSize.Width > 814.0 ? (814.0 / SubTitle.TextureSize.Width) : 1.0;
                            Genre.Draw(difXY[0] + 407 - (Genre.ActualSize.Width / 2), 190);
                            Title.Draw(difXY[0] + 407 - (Title.ActualSize.Width / 2), 240);
                            SubTitle.Draw(difXY[0] + 407 - (SubTitle.ActualSize.Width / 2), 340);
                            BPM.Draw(difXY[0] + 407 - (BPM.ActualSize.Width / 2), 390);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(NowTJA.Header.GENRE)) DrawString(difXY[0] + 407 - (GetDrawStringWidth(NowTJA.Header.GENRE, NowTJA.Header.GENRE.Length) / 2), 190, NowTJA.Header.GENRE, 0xffffff);
                            if (!string.IsNullOrEmpty(NowTJA.Header.TITLE)) DrawString(difXY[0] + 407 - (GetDrawStringWidth(NowTJA.Header.TITLE, NowTJA.Header.TITLE.Length) / 2), 240, NowTJA.Header.TITLE, 0xffffff);
                            if (!string.IsNullOrEmpty(NowTJA.Header.SUBTITLE)) DrawString(difXY[0] + 407 - (GetDrawStringWidth(NowTJA.Header.SUBTITLE, NowTJA.Header.SUBTITLE.Length) / 2), 340, NowTJA.Header.SUBTITLE, 0xffffff);
                            string bpm = $"{Math.Round(NowTJA.Header.BPM, 0, MidpointRounding.AwayFromZero)} BPM";
                            DrawString(difXY[0] + 407 - (GetDrawStringWidth(bpm, bpm.Length) / 2), 390, bpm, 0xffffff);
                        }
                        bool select = Mouse.X >= difXY[0] + 22 && Mouse.X < difXY[0] + 795 && Mouse.Y >= difXY[1] + 8 && Mouse.Y < difXY[1] + 126;
                        double widgh = (Mouse.X - (difXY[0] + 22 + 74.5)) / 156;
                        int wcursor = (int)Math.Round(widgh, 0, MidpointRounding.AwayFromZero);
                        for (int i = 0; i < 5; i++)
                        {
                            if (i == (select && !Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[0]) || (PlayData.Data.IsPlay2P && i == (select && Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[1])) || PlayData.Data.PreviewType == 3)
                            {
                                switch (i)
                                {
                                    case (int)ECourse.Easy:
                                        TextureLoad.SongSelect_Difficulty.Color = Color.FromArgb(0xff5f3f);
                                        break;
                                    case (int)ECourse.Normal:
                                        TextureLoad.SongSelect_Difficulty.Color = Color.FromArgb(0x9fff3f);
                                        break;
                                    case (int)ECourse.Hard:
                                        TextureLoad.SongSelect_Difficulty.Color = Color.FromArgb(0x3fbfff);
                                        break;
                                    case (int)ECourse.Oni:
                                        TextureLoad.SongSelect_Difficulty.Color = Color.FromArgb(0xff3f9f);
                                        break;
                                    case (int)ECourse.Edit:
                                        TextureLoad.SongSelect_Difficulty.Color = Color.FromArgb(0x9f3fff);
                                        break;
                                }
                            }
                            else
                            {
                                switch (i)
                                {
                                    case (int)ECourse.Easy:
                                        TextureLoad.SongSelect_Difficulty.Color = Color.FromArgb(0x7f2f1f);
                                        break;
                                    case (int)ECourse.Normal:
                                        TextureLoad.SongSelect_Difficulty.Color = Color.FromArgb(0x4f7f1f);
                                        break;
                                    case (int)ECourse.Hard:
                                        TextureLoad.SongSelect_Difficulty.Color = Color.FromArgb(0x1f5f7f);
                                        break;
                                    case (int)ECourse.Oni:
                                        TextureLoad.SongSelect_Difficulty.Color = Color.FromArgb(0x7f1f4f);
                                        break;
                                    case (int)ECourse.Edit:
                                        TextureLoad.SongSelect_Difficulty.Color = Color.FromArgb(0x4f1f7f);
                                        break;
                                }
                            }
                            if (NowTJA.Course[i].IsEnable)
                            {
                                TextureLoad.SongSelect_Difficulty.Draw(difXY[0] + 22 + 156 * i, difXY[1] + 8);
                                TextureLoad.SongSelect_Difficulty_Course.Draw(difXY[0] + 22 + 156 * i, difXY[1] + 100);
                                Score.DrawNumber(difXY[0] + 94 + 156 * i - 12 * Score.Digit(NowTJA.Course[i].LEVEL), difXY[1] + 42, $"{NowTJA.Course[i].LEVEL}", 0);
                            }
                        }
                        if (NowTJA.Course[select && !Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[0]].LEVEL > 0)
                        {
                            int lev = NowTJA.Course[select && !Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[0]].LEVEL < 12 ? NowTJA.Course[select && !Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[0]].LEVEL : 12;
                            for (int i = 0; i < lev; i++)
                            {
                                switch (select && !Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[0])
                                {
                                    case (int)ECourse.Easy:
                                        TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0xff5f3f);
                                        break;
                                    case (int)ECourse.Normal:
                                        TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x9fff3f);
                                        break;
                                    case (int)ECourse.Hard:
                                        TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x3fbfff);
                                        break;
                                    case (int)ECourse.Oni:
                                        TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0xff3f9f);
                                        break;
                                    case (int)ECourse.Edit:
                                        TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x9f3fff);
                                        break;
                                }
                                TextureLoad.SongSelect_Difficulty_Level.Draw(difXY[0] + 12 + 66 * i, difXY[1] + 136);
                                if (NowTJA.Course[PlayData.Data.PlayCourse[0]].LEVEL > 12)
                                {

                                    for (int j = 0; j < NowTJA.Course[select && !Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[0]].LEVEL - 12; j++)
                                    {
                                        switch (select && !Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[0])
                                        {
                                            case (int)ECourse.Easy:
                                                TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x7f2f1f);
                                                break;
                                            case (int)ECourse.Normal:
                                                TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x4f7f1f);
                                                break;
                                            case (int)ECourse.Hard:
                                                TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x1f5f7f);
                                                break;
                                            case (int)ECourse.Oni:
                                                TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x7f1f4f);
                                                break;
                                            case (int)ECourse.Edit:
                                                TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x4f1f7f);
                                                break;
                                        }
                                        TextureLoad.SongSelect_Difficulty_Level.Draw(difXY[0] + 12 + 66 * j, difXY[1] + 136);
                                    }
                                }
                            }
                        }
                        if (PlayData.Data.IsPlay2P && NowTJA.Course[select && Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[1]].LEVEL > 0)
                        {
                            int lev = NowTJA.Course[select && Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[1]].LEVEL < 12 ? NowTJA.Course[select && Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[1]].LEVEL : 12;
                            for (int i = 0; i < lev; i++)
                            {
                                switch (select && Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[1])
                                {
                                    case (int)ECourse.Easy:
                                        TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0xff5f3f);
                                        break;
                                    case (int)ECourse.Normal:
                                        TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x9fff3f);
                                        break;
                                    case (int)ECourse.Hard:
                                        TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x3fbfff);
                                        break;
                                    case (int)ECourse.Oni:
                                        TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0xff3f9f);
                                        break;
                                    case (int)ECourse.Edit:
                                        TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x9f3fff);
                                        break;
                                }
                                TextureLoad.SongSelect_Difficulty_Level.Draw(difXY[0] + 12 + 66 * i, difXY[1] + 136 + 31);
                                if (NowTJA.Course[select && Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[1]].LEVEL > 12)
                                {

                                    for (int j = 0; j < NowTJA.Course[select && Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[1]].LEVEL - 12; j++)
                                    {
                                        switch (select && Key.IsPushing(KEY_INPUT_LSHIFT) ? wcursor : PlayData.Data.PlayCourse[1])
                                        {
                                            case (int)ECourse.Easy:
                                                TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x7f2f1f);
                                                break;
                                            case (int)ECourse.Normal:
                                                TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x4f7f1f);
                                                break;
                                            case (int)ECourse.Hard:
                                                TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x1f5f7f);
                                                break;
                                            case (int)ECourse.Oni:
                                                TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x7f1f4f);
                                                break;
                                            case (int)ECourse.Edit:
                                                TextureLoad.SongSelect_Difficulty_Level.Color = Color.FromArgb(0x4f1f7f);
                                                break;
                                        }
                                        TextureLoad.SongSelect_Difficulty_Level.Draw(difXY[0] + 12 + 66 * j, difXY[1] + 136 + 31);
                                    }
                                }
                            }
                        }

                        DrawString(480, 640, "REPLAYDATA MENU (Beta mode)", 0xffffff);
                        if (!string.IsNullOrEmpty(PlayData.Data.BestScore) && File.Exists($@"{Path.GetDirectoryName(NowTJA.Path)}\{Path.GetFileNameWithoutExtension(NowTJA.Path)}.{(ECourse)PlayData.Data.PlayCourse[0]}.{PlayData.Data.BestScore}.replaydata"))
                        {
                            DrawString(480, 680, $"BestScore:{PlayData.Data.BestScore}", 0xffffff);
                        }
                        else
                        {
                            DrawString(480, 680, "BestScore:None", 0xffffff);
                        }
                        if (!string.IsNullOrEmpty(PlayData.Data.RivalScore) && File.Exists($@"{Path.GetDirectoryName(NowTJA.Path)}\{Path.GetFileNameWithoutExtension(NowTJA.Path)}.{(ECourse)PlayData.Data.PlayCourse[0]}.{PlayData.Data.RivalScore}.replaydata"))
                        {
                            DrawString(480, 700, $"RivalScore:{PlayData.Data.RivalScore}", 0xffffff);
                        }
                        else
                        {
                            DrawString(480, 700, "RivalScore:None", 0xffffff);
                        }
                        if (!string.IsNullOrEmpty(PlayData.Data.Replay[0]) && File.Exists($@"{Path.GetDirectoryName(NowTJA.Path)}\{Path.GetFileNameWithoutExtension(NowTJA.Path)}.{(ECourse)PlayData.Data.PlayCourse[0]}.{PlayData.Data.Replay[0]}.replaydata"))
                        {
                            DrawString(480, 720, $"ReplayScore1P:{PlayData.Data.Replay[0]}", 0xffffff);
                            DrawString(800, 720, "Press LSHIFT & ENTER to playback", 0xffffff);
                        }
                        else
                        {
                            DrawString(480, 720, "ReplayScore1P:None", 0xffffff);
                        }
                        if (PlayData.Data.IsPlay2P)
                        {
                            if (!string.IsNullOrEmpty(PlayData.Data.Replay[1]) && File.Exists($@"{Path.GetDirectoryName(NowTJA.Path)}\{Path.GetFileNameWithoutExtension(NowTJA.Path)}.{(ECourse)PlayData.Data.PlayCourse[1]}.{PlayData.Data.Replay[1]}.replaydata"))
                            {
                                DrawString(480, 740, $"ReplayScore2P:{PlayData.Data.Replay[1]}", 0xffffff);
                                DrawString(800, 740, "Press RSHIFT & ENTER to playback", 0xffffff);
                            }
                            else
                            {
                                DrawString(480, 740, "ReplayScore2P:None", 0xffffff);
                            }
                        }
                        break;
                    default:
                        break;
                }

                for (int i = 0; i < 5; i++)
                {
                    if ((i == PlayData.Data.PlayCourse[0] || (PlayData.Data.IsPlay2P && i == PlayData.Data.PlayCourse[1])) && PlayData.Data.PreviewType < 3)
                    {
                        TextureLoad.SongSelect_Difficulty_Cursor.Draw(difXY[0] + 22 + 156 * i, difXY[1] + 8);
                    }
                }
                TextureLoad.SongSelect_Difficulty_TJA.Draw(difXY[0], difXY[1]);

                DrawString(80, 640, "OPTION MENU (Beta mode)", 0xffffff);
                DrawString(80, 680, "1P", 0xffffff);
                if (PlayData.Data.Auto[0])
                {
                    DrawString(110, 680, "AUTO", 0xffffff);
                }
                DrawString(80, 700, $"Gauge:{(EGauge)PlayData.Data.GaugeType[0]}", 0xffffff);
                DrawString(80, 720, $"GAS:{(EGaugeAutoShift)PlayData.Data.GaugeAutoShift[0]}", 0xffffff);
                DrawString(80, 740, $"GASmin:{(EGauge)PlayData.Data.GaugeAutoShiftMin[0]}", 0xffffff);
                DrawString(80, 760, $"Hazard:{PlayData.Data.Hazard[0]}", 0xffffff);
                if (PlayData.Data.IsPlay2P)
                {
                    DrawString(240, 680, "2P", 0xffffff);
                    if (PlayData.Data.Auto[1])
                    {
                        DrawString(270, 680, "AUTO", 0xffffff);
                    }
                    DrawString(240, 700, $"Gauge:{(EGauge)PlayData.Data.GaugeType[1]}", 0xffffff);
                    DrawString(240, 720, $"GAS:{(EGaugeAutoShift)PlayData.Data.GaugeAutoShift[1]}", 0xffffff);
                    DrawString(240, 740, $"GASmin:{(EGauge)PlayData.Data.GaugeAutoShiftMin[1]}", 0xffffff);
                    DrawString(240, 760, $"Hazard:{PlayData.Data.Hazard[1]}", 0xffffff);
                }
            }
            else
            {
                DrawBox(0, 308, 530, 348, 0x000000, TRUE);
                DrawString(80, 320, "TJAがありません。パスを確認し、ロードしてください。", 0xffffff);
            }

            if (Input.IsEnable)
            {
                DrawBox(0, 1040, GetDrawStringWidth(Input.Text, Input.Position) + 50, 1080, 0x000000, TRUE);
                DrawString(20, 1052, Input.Text, 0xffffff);
                DrawString(16 + GetDrawStringWidth(Input.Text, Input.Position), 1052, "|", 0xffff00);
            }

            if (Alart.State != 0)
            {
                DrawBox(0, 1040, 410, 1080, 0x000000, TRUE);
                switch (AlartType)
                {
                    case 0:
                        DrawString(20, 1052, "TJAが見つかりません!パスを確認してください。", 0xffffff);
                        break;
                    case 1:
                        DrawString(20, 1052, "譜面がありません!難易度を確認してください。", 0xffffff);
                        break;
                    case 2:
                        if (SongLoad.NowSort != ESort.None)
                        {
                            DrawString(20, 1052, $"譜面を並び替えました! Sort:{SongLoad.NowSort}...", 0xffffff);
                        }
                        else
                        {
                            DrawString(20, 1052, $"譜面の並び順をデフォルトに戻しました!", 0xffffff);
                        }
                        break;
                }
            }

            #if DEBUG
            if (NowTJA != null)DrawString(0, 0, $"File:{NowTJA.Path}", 0xffffff);
            DrawString(200, 20, $"SongCount:{SongLoad.SongData.Count},{SongLoad.SongList.Count}", 0xffffff);
            DrawString(200, 40, $"SongNumber:{NowSongNumber}", 0xffffff);
            DrawString(0, 20, $"Course:{(ECourse)PlayData.Data.PlayCourse[0]}" + (PlayData.Data.IsPlay2P ? $"/{(ECourse)PlayData.Data.PlayCourse[1]}" : ""), 0xffffff);
            if (PlayData.Data.Auto[0])
            {
                DrawString(0, 40, "1P AUTO", 0xffffff);
            }
            if (PlayData.Data.Auto[1] && PlayData.Data.IsPlay2P)
            {
                DrawString(80, 40, "2P AUTO", 0xffffff);
            }
            double length = (Mouse.Y - 540.0) / TextureLoad.SongSelect_Bar.ActualSize.Height;
            int cursor = (int)Math.Round(length, 0, MidpointRounding.AwayFromZero);
            DrawString(200, 60, $"NowY:{length}", 0xffffff);
            DrawString(200, 80, $"NowYCursor:{cursor}", 0xffffff);
            DrawString(200, 100, $"NowWheel:{Mouse.Wheel}", 0xffffff);
            DrawString(0, 60, $"Gauge:{(EGauge)PlayData.Data.GaugeType[0]}" + (PlayData.Data.IsPlay2P ? $"/{(EGauge)PlayData.Data.GaugeType[1]}" : ""), 0xffffff);
            DrawString(0, 80, $"GAS:{(EGaugeAutoShift)PlayData.Data.GaugeAutoShift[0]}" + (PlayData.Data.IsPlay2P ? $"/{(EGaugeAutoShift)PlayData.Data.GaugeAutoShift[1]}" : ""), 0xffffff);
            DrawString(0, 100, $"GASmin:{(EGauge)PlayData.Data.GaugeAutoShiftMin[0]}" + (PlayData.Data.IsPlay2P ? $"/{(EGauge)PlayData.Data.GaugeAutoShiftMin[1]}" : ""), 0xffffff);
            DrawString(0, 120, $"Hazard:{PlayData.Data.Hazard[0]}" + (PlayData.Data.IsPlay2P ? $"/{PlayData.Data.Hazard[1]}" : ""), 0xffffff);

            DrawString(0, 140, "PRESS ENTER", 0xffffff);
            #endif
            base.Draw();
        }

        public override void Update()
        {
            Alart.Tick();
            if (Alart.Value == Alart.End) Alart.Stop();

            if (Input.IsEnable)
            {
                if (Key.IsPushed(KEY_INPUT_RETURN))
                {
                    Input.End();
                }
            }
            else
            {
                if (Key.IsPushed(PlayData.Data.LEFTKA))
                {
                    PushedTimer[0].Start();
                }
                if (Key.IsLeft(PlayData.Data.LEFTKA))
                {
                    if (PlayData.Data.FontRendering) FontLoad();
                    PushedTimer[0].Stop();
                    PushedTimer[0].Reset();
                    PushingTimer[0].Stop();
                    PushingTimer[0].Reset();
                    
                }
                if (Key.IsPushed(PlayData.Data.RIGHTKA))
                {
                    PushedTimer[1].Start();
                }
                if (Key.IsLeft(PlayData.Data.RIGHTKA))
                {
                    if (PlayData.Data.FontRendering) FontLoad();
                    PushedTimer[1].Stop();
                    PushedTimer[1].Reset();
                    PushingTimer[1].Stop();
                    PushingTimer[1].Reset();
                }
                for (int i = 0; i < 2; i++)
                {
                    if (PushedTimer[i].Value == PushedTimer[i].End)
                    {
                        PushingTimer[i].Start();
                    }
                }

                if ((Key.IsPushed(PlayData.Data.LEFTKA) || (PushingTimer[0].Value == PushingTimer[0].End) || Mouse.Wheel < 0) && NowTJA != null)
                {
                    SoundLoad.Ka[0].Play();
                    NowTJA = NowTJA.Prev;
                    if (NowSongNumber <= 0) NowSongNumber = SongLoad.SongData.Count - 1;
                    else NowSongNumber--;
                    PushingTimer[0].Reset();
                }
                if ((Key.IsPushed(PlayData.Data.RIGHTKA) || (PushingTimer[1].Value == PushingTimer[1].End) || Mouse.Wheel > 0) && NowTJA != null)
                {
                    SoundLoad.Ka[0].Play();
                    NowTJA = NowTJA.Next;
                    if (NowSongNumber >= SongLoad.SongData.Count - 1) NowSongNumber = 0;
                    else NowSongNumber++;
                    PushingTimer[1].Reset();
                }
                if (((Key.IsPushing(PlayData.Data.LEFTKA) && Key.IsPushed(PlayData.Data.RIGHTKA)) || (Key.IsPushed(PlayData.Data.LEFTKA) && Key.IsPushing(PlayData.Data.RIGHTKA))) && SongLoad.FolderFloor > 0)
                {
                    NowTJA = SongLoad.SongData[0];
                    string title = NowTJA.Title;
                    SongLoad.FolderFloor--;
                    SongLoad.SongData = new List<SongData>();
                    SongLoad.FolderData = new List<string>();
                    NowPath = NowTJA.Path;
                    SongLoad.Load(SongLoad.SongData, NowTJA.Path);
                    for (int i = 0; i < SongLoad.SongData.Count; i++)
                    {
                        if (SongLoad.SongData[i].Title == title)
                        {
                            NowSongNumber = i;
                            break;
                        }
                    }
                    NowTJA = SongLoad.SongData[NowSongNumber];
                    if (PlayData.Data.FontRendering) FontLoad();
                }

                if ((Key.IsPushed(KEY_INPUT_RETURN) || Key.IsPushed(PlayData.Data.LEFTDON) || Key.IsPushed(PlayData.Data.RIGHTDON)) && NowTJA != null)
                {
                    SoundLoad.Don[0].Play();
                    Enter();
                }

                int difx = 72, dify = 500 - 60;
                double length = (Mouse.Y - 540.0) / TextureLoad.SongSelect_Bar.ActualSize.Height;
                int cursor = (int)Math.Round(length, 0, MidpointRounding.AwayFromZero);
                double widgh = (Mouse.X - (difx + 22 + 74.5)) / 156;
                int wcursor = (int)Math.Round(widgh, 0, MidpointRounding.AwayFromZero);
                if (Mouse.IsPushed(MouseButton.Left) && NowTJA != null)
                {
                    if (Mouse.X >= 1212 || (cursor == 0 && Mouse.X >= 1180))
                    {
                        if (cursor < 0)
                        {
                            SoundLoad.Ka[0].Play();
                            for (int i = 0; i < -cursor; i++)
                            {
                                if (NowSongNumber <= 0) NowSongNumber = SongLoad.SongData.Count - 1;
                                else NowSongNumber--;
                            }
                            NowTJA = SongLoad.SongData[NowSongNumber];
                        }
                        else if (cursor > 0)
                        {
                            SoundLoad.Ka[0].Play();
                            for (int i = 0; i < cursor; i++)
                            {
                                if (NowSongNumber >= SongLoad.SongData.Count - 1) NowSongNumber = 0;
                                else NowSongNumber++;
                            }
                            NowTJA = SongLoad.SongData[NowSongNumber];
                        }
                        else
                        {
                            SoundLoad.Don[0].Play();
                            Enter();
                        }
                    }
                    else if (Mouse.X >= difx + 22 && Mouse.X < difx + 795 && Mouse.Y >= dify + 8 && Mouse.Y < dify + 126)
                    {
                        SoundLoad.Ka[0].Play();
                        if (Key.IsPushing(KEY_INPUT_LSHIFT) && PlayData.Data.IsPlay2P)
                        {
                            PlayData.Data.PlayCourse[1] = wcursor;
                        }
                        else
                        {
                            PlayData.Data.PlayCourse[0] = wcursor;
                        }
                    }
                    else
                    {
                        if (SongLoad.FolderFloor > 0)
                        {
                            SoundLoad.Ka[0].Play();
                            NowTJA = SongLoad.SongData[0];
                            string title = NowTJA.Title;
                            SongLoad.FolderFloor--;
                            SongLoad.SongData = new List<SongData>();
                            SongLoad.SongList = new List<SongData>();
                            SongLoad.FolderData = new List<string>();
                            NowPath = NowTJA.Path;
                            SongLoad.Load(SongLoad.SongData, NowTJA.Path);
                            for (int i = 0; i < SongLoad.SongData.Count; i++)
                            {
                                if (SongLoad.SongData[i].Title == title)
                                {
                                    NowSongNumber = i;
                                    break;
                                }
                            }
                            NowTJA = SongLoad.SongData[NowSongNumber];
                            if (PlayData.Data.FontRendering) FontLoad();
                        }
                    }
                }

                if (Key.IsPushed(KEY_INPUT_SPACE))
                {
                    SoundLoad.Ka[0].Play();
                    string title = NowTJA.Title;
                    if (SongLoad.NowSort == ESort.Rate_Low) SongLoad.NowSort = ESort.None;
                    else SongLoad.NowSort++;
                    SongLoad.Sort(SongLoad.SongData, SongLoad.NowSort);
                    SongLoad.Sort(SongLoad.SongList, SongLoad.NowSort);

                    for (int i = 0; i < SongLoad.SongData.Count; i++)
                    {
                        if (SongLoad.SongData[i].Title == title)
                        {
                            NowSongNumber = i;
                            break;
                        }
                    }
                    NowTJA = SongLoad.SongData[NowSongNumber];
                    if (PlayData.Data.FontRendering) FontLoad();
                    AlartType = 2;
                    Alart.Reset();
                    Alart.Start();
                }

                if (Key.IsPushed(KEY_INPUT_ESCAPE))
                {
                    Program.SceneChange(new Title.Title());
                }
                if (Key.IsPushing(KEY_INPUT_LSHIFT) && Key.IsPushing(KEY_INPUT_RSHIFT) && Key.IsPushing(KEY_INPUT_DELETE))
                {
                    Program.SceneChange(new Game.Game());
                }

                if (Key.IsPushed(KEY_INPUT_F1))
                {
                    Program.SceneChange(new Config.Config());
                }
                if (Key.IsPushed(KEY_INPUT_F2))
                {
                    PlayData.Init();
                    SongLoad.Init();
                }
                if (Key.IsPushed(KEY_INPUT_F3))
                {
                    PlayData.Data.Auto[0] = !PlayData.Data.Auto[0];
                }
                if (Key.IsPushed(KEY_INPUT_F4) && PlayData.Data.IsPlay2P)
                {
                    PlayData.Data.Auto[1] = !PlayData.Data.Auto[1];
                }
                if (Key.IsPushed(KEY_INPUT_F5))
                {
                    if (Key.IsPushing(KEY_INPUT_LSHIFT) || Key.IsPushing(KEY_INPUT_RSHIFT) && PlayData.Data.IsPlay2P)
                    {
                        GaugeChange(1);
                    }
                    else
                    {
                        GaugeChange(0);
                    }
                }
                if (Key.IsPushed(KEY_INPUT_F6))
                {
                    if (Key.IsPushing(KEY_INPUT_LSHIFT) || Key.IsPushing(KEY_INPUT_RSHIFT) && PlayData.Data.IsPlay2P)
                    {
                        GASChange(1);
                    }
                    else
                    {
                        GASChange(0);
                    }
                }
                if (Key.IsPushed(KEY_INPUT_F7))
                {
                    PlayData.Data.IsPlay2P = !PlayData.Data.IsPlay2P;
                }
                if (Key.IsPushed(KEY_INPUT_F8))
                {
                    PlayData.Data.ShowResultScreen = !PlayData.Data.ShowResultScreen;
                }
                if (Key.IsPushed(KEY_INPUT_F9))
                {
                    if ((Key.IsPushing(KEY_INPUT_LSHIFT) || Key.IsPushing(KEY_INPUT_RSHIFT)) && PlayData.Data.IsPlay2P)
                    {
                        PlayData.Data.Hazard[1]--;
                    }
                    else
                    {
                        PlayData.Data.Hazard[0]--;
                    }
                }
                if (Key.IsPushed(KEY_INPUT_F10))
                {
                    if ((Key.IsPushing(KEY_INPUT_LSHIFT) || Key.IsPushing(KEY_INPUT_RSHIFT)) && PlayData.Data.IsPlay2P)
                    {
                        PlayData.Data.Hazard[1]++;
                    }
                    else
                    {
                        PlayData.Data.Hazard[0]++;
                    }
                }

                if (Key.IsPushed(KEY_INPUT_LEFT))
                {
                    SoundLoad.Ka[0].Play();
                    if ((Key.IsPushing(KEY_INPUT_LSHIFT) || Key.IsPushing(KEY_INPUT_RSHIFT)) && PlayData.Data.IsPlay2P)
                    {
                        CourseChange(true, 1);
                    }
                    else
                    {
                        CourseChange(true, 0);
                    }
                }
                if (Key.IsPushed(KEY_INPUT_RIGHT))
                {
                    SoundLoad.Ka[0].Play();
                    if ((Key.IsPushing(KEY_INPUT_LSHIFT) || Key.IsPushing(KEY_INPUT_RSHIFT)) && PlayData.Data.IsPlay2P)
                    {
                        CourseChange(false, 1);
                    }
                    else
                    {
                        CourseChange(false, 0);
                    }
                }
                if (Key.IsPushed(KEY_INPUT_SLASH))
                {
                    Input.Init();
                }
            }

            for (int i = 0; i < 2; i++)
            {
                PushedTimer[i].Tick();
                PushingTimer[i].Tick();
            }
            base.Update();
        }

        public static void Enter()
        {
            switch (NowTJA.Type)
            {
                case EType.Score:
                    if (File.Exists(NowTJA.Path))
                    {
                        if (NowTJA.Course[PlayData.Data.PlayCourse[0]].IsEnable || PlayData.Data.PreviewType == 3)
                        {
                            if (Key.IsPushing(KEY_INPUT_LSHIFT))
                            {
                                Replay[0] = true;
                            }
                            else
                            {
                                Replay[0] = false;
                            }
                            Random = false;
                            NowSListNumber = NowSongNumber;
                            for (int i = 0; i < SongLoad.SongData.Count; i++)
                            {
                                if (SongLoad.SongData[i].Type != EType.Score)
                                    NowSListNumber--;
                            }
                            Program.SceneChange(new Game.Game());
                        }
                        else
                        {
                            AlartType = 1;
                            Alart.Reset();
                            Alart.Start();
                        }
                    }
                    else
                    {
                        AlartType = 0;
                        Alart.Reset();
                        Alart.Start();
                    }
                    break;
                case EType.Random:
                    for (int i = 0; i < 100000000; i++)
                    {
                        Random random = new Random();
                        int r = random.Next(SongLoad.SongList.Count);
                        if (SongLoad.SongList[r] != null && SongLoad.SongList[r].Course[PlayData.Data.PlayCourse[0]].IsEnable)
                        {
                            NowTJA = SongLoad.SongList[r];
                            break;
                        }
                    }
                    if (File.Exists(NowTJA.Path))
                    {
                        if (NowTJA.Course[PlayData.Data.PlayCourse[0]].IsEnable || PlayData.Data.PreviewType == 3)
                        {
                            if (Key.IsPushing(KEY_INPUT_LSHIFT))
                            {
                                Replay[0] = true;
                            }
                            else
                            {
                                Replay[0] = false;
                            }
                            Random = true;
                            Program.SceneChange(new Game.Game());
                        }
                        else
                        {
                            AlartType = 1;
                            Alart.Reset();
                            Alart.Start();
                        }
                    }
                    else
                    {
                        AlartType = 0;
                        Alart.Reset();
                        Alart.Start();
                    }
                    break;
                case EType.Folder:
                    SongLoad.FolderFloor++;
                    SongLoad.SongData = new List<SongData>();
                    SongLoad.SongList = new List<SongData>();
                    SongLoad.FolderData = new List<string>();
                    NowPath = NowTJA.Path;
                    SongLoad.Load(SongLoad.SongData, NowTJA.Path);
                    NowSongNumber = 0;
                    NowTJA = SongLoad.SongData[NowSongNumber];
                    if (PlayData.Data.FontRendering) FontLoad();
                    break;
                case EType.Back:
                    string title = NowTJA.Title;
                    SongLoad.FolderFloor--;
                    SongLoad.SongData = new List<SongData>();
                    SongLoad.SongList = new List<SongData>();
                    SongLoad.FolderData = new List<string>();
                    NowPath = NowTJA.Path;
                    SongLoad.Load(SongLoad.SongData, NowTJA.Path);
                    for (int i = 0; i < SongLoad.SongData.Count; i++)
                    {
                        if (SongLoad.SongData[i].Title == title)
                        {
                            NowSongNumber = i;
                            break;
                        }
                    }
                    NowTJA = SongLoad.SongData[NowSongNumber];
                    if (PlayData.Data.FontRendering) FontLoad();
                    break;
            }
        }

        public static void CourseChange(bool isdim, int player)
        {
            if (!isdim)
            {
                #region [ 上げる ]
                if (PlayData.Data.PlayCourse[player] < 4)
                    PlayData.Data.PlayCourse[player]++;
                else
                    PlayData.Data.PlayCourse[player] = 0;
                #endregion
            }
            else
            {
                #region [ 下げる ]
                if (PlayData.Data.PlayCourse[player] > 0)
                    PlayData.Data.PlayCourse[player]--;
                else
                    PlayData.Data.PlayCourse[player] = 4;
                #endregion
            }

        }

        public static void FontLoad()
        {
            if (NowTJA.Header == null) return;

            FontFamily family = new FontFamily("MS UI Gothic");
            if (!string.IsNullOrEmpty(NowTJA.Header.TITLE))
            {
                Title = DrawFont.GetTexture(NowTJA.Header.TITLE, family, 96, 4, 0, Color.White, Color.Black);
            }
            else Title = new Texture();
            FontFamily familyb = new FontFamily("Arial Black");
            if (!string.IsNullOrEmpty(NowTJA.Header.GENRE))
            {
                Genre = DrawFont.GetTexture(NowTJA.Header.GENRE, familyb, 32, 4, 0, Color.White, Color.Black);
            }
            else Genre = new Texture();
            if (!string.IsNullOrEmpty(NowTJA.Header.SUBTITLE))
            {
                SubTitle = DrawFont.GetTexture(NowTJA.Header.SUBTITLE, familyb, 32, 4, 0, Color.White, Color.Black);
            }
            else SubTitle = new Texture();
            FontFamily familyc = new FontFamily("Arial Black");
            BPM = DrawFont.GetTexture($"{Math.Round(NowTJA.Header.BPM, 0, MidpointRounding.AwayFromZero)} BPM", familyc, 32, 6, 0, Color.White, Color.Black);
        }

        public static void GaugeChange(int player)
        {
            if (PlayData.Data.GaugeType[player] < (int)EGauge.EXHard)
                PlayData.Data.GaugeType[player]++;
            else
                PlayData.Data.GaugeType[player] = (int)EGauge.Normal;
        }
        public static void GASChange(int player)
        {
            if (PlayData.Data.GaugeAutoShift[player] < (int)EGaugeAutoShift.LessBest)
                PlayData.Data.GaugeAutoShift[player]++;
            else
                PlayData.Data.GaugeAutoShift[player] = (int)EGaugeAutoShift.None;
        }

        public static bool[] Replay = new bool[2];
        public static Texture Title, SubTitle, Genre, BPM;
        public static SongData NowTJA;
        public static Counter Alart;
        public static Counter[] PushedTimer = new Counter[2], PushingTimer = new Counter[2];
        public static int AlartType, NowSongNumber, NowSListNumber;
        public static bool Random;
        public static string NowPath;
    }
}
