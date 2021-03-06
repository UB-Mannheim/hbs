﻿// OpenBookTransition.cs
// Date Created: 20.01.2016
// 
// Copyright (c) 2016, picibird GmbH 
// All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using picibird.hbs.models;
using picibird.hbs.viewmodels.book3D;
using picibits.app.transition;

namespace picibird.hbs.transition
{
    public class OpenBookTransition : TransitionBase
    {
        public OpenBookTransition(Book3DViewModel book3D, Book book)
        {
            if (book == null || book3D == null)
                throw new ArgumentException("book and book3d cannot be null");
            Book3D = book3D;
            Book = book;
        }

        public Book3DViewModel Book3D { get; set; }
        public Book Book { get; set; }

        public override void OnTransitionStarting()
        {
            Book3D.Visibility = false;

            HBS.ViewModel.BookFlyout3dVM.Book = Book;
            HBS.ViewModel.BookFlyout3dVM.ClosedBook3dMatrix = Book3D.TransformMatrix3D;
            HBS.ViewModel.BookFlyout3dVM.Visibility = true;

            HBS.ViewModel.BlackBlendingViewModel.IsHitTestVisible = true;
            HBS.ViewModel.ShelfViewModel.IsBitmapCacheEnabled = true;
            HBS.IsAnimating = true;
        }

        public override void OnTransitionProgress(double progress)
        {
            HBS.ViewModel.BlackBlendingViewModel.Opacity = progress*HBS.BlackBlendingOpacity;
            HBS.ViewModel.BookFlyout3dVM.Progress = 1 - progress;
        }

        public override void OnTranistionCompleted()
        {
            HBS.ViewModel.Opened.BookVM.FrontCover.Style = HBS.ViewModel.BookFlyout3dVM.OpenedBook.FrontCover.Style;
            HBS.ViewModel.Opened.BookVM.Spine.Style = HBS.ViewModel.BookFlyout3dVM.OpenedBook.Spine.Style;
            HBS.ViewModel.Opened.BookVM.BackCover.Style = HBS.ViewModel.BookFlyout3dVM.OpenedBook.BackCover.Style;

            HBS.ViewModel.Opened.BookVM.Book = Book;
            HBS.ViewModel.Opened.BookVM.Book3D = Book3D;

            HBS.ViewModel.Opened.UpdatePositionAndSize(HBS.ViewModel.BookFlyout3dVM.OpenedBookRect);
            HBS.ViewModel.Opened.Visibility = true;

            HBS.ViewModel.Opened.BookVM.AnimateToDropShadow();
            HBS.IsAnimating = false;

            Book.WriteNfcUri();
        }
    }
}