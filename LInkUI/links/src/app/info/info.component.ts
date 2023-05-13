import { Component } from '@angular/core';
import { Link } from '../models/link.model';
import { LinksService } from '../services/links.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-info',
  templateUrl: './info.component.html',
  styleUrls: ['./info.component.css']
})
export class InfoComponent {
  title = "Url Info";
  displayedColumns: string[] = [
    'shortLink',
    'date',
    'createdBy',
    'action',
  ];

  linksData: Link[] = [];
  buttonLabel: string = 'Copy';

  constructor(
    private _linkService: LinksService,
    private datepipe: DatePipe,
  ) { }

  ngOnInit(): void {
    this.getLinks();
  }
  getLinks() {
    this._linkService.getLinks().subscribe(
      response => {
        response.forEach(element => {
          element.date = this.datepipe.transform(element.date, 'yyyy-MM-dd \n HH:mm:ss') as any;
        });
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
    this.ngOnInit();
  }
}
