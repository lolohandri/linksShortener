import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { LinksComponent } from '../links/links.component';
import { LoginComponent } from '../login/login.component';
import { Link } from '../models/link.model';
import { LinksService } from '../services/links.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  title = 'Url Shortener';
  displayedColumns: string[] = [
    'originLink',
    'shortLink',
    'action',
  ];

  linksData: Link[] = [];
  token: string = localStorage.getItem('userToken') as any;

  constructor(
    private _linkService: LinksService,
  ) { }

  ngOnInit(): void {
    this.getLinks();

  }
  getLinks() {
    this._linkService.getLinks().subscribe(
      response => {
        this.linksData = response;
      }
    );
  }
  onCopy(event: Event): void {
    const value = event.target as HTMLButtonElement;
    value.textContent = 'Copied';
    setTimeout(() => {
      value.textContent = 'Copy';
    }, 1000);
  }
  onDelete(id: number): void {
    this._linkService.deleteLink(id).subscribe(
      response => {
        console.log(response);
        this.getLinks();
      }
    );
  }
}
