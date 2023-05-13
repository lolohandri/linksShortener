import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LinksComponent } from './links/links.component';
import { LinksService } from './services/links.service';
import { Link } from './models/link.model';
import { LoginComponent } from './login/login.component';
import { DatePipe } from '@angular/common'
import { Router } from '@angular/router';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    title = 'Url Shortener';
    linkDialog: MatDialog;
    loginDialog: MatDialog;
    displayedColumns: string[] = [
        'id',
        'originLink',
        'shortLink',
    ];
    linksData: Link[] = [];
    token: any;
    decodedToken: any;

    constructor(
        private _linkDialog: MatDialog,
        private _loginDialog: MatDialog,
        private _linkService: LinksService,
        private ref: ChangeDetectorRef,
        private datepipe: DatePipe,
        private router: Router,
    ) {
        this.linkDialog = _linkDialog;
        this.loginDialog = _loginDialog;
    }

    ngOnInit(): void {
        this.token = localStorage.getItem('userToken');
        if (this.token != null) {
            let tokenDecoded = this.getDecodedAccessToken(this.token);
            localStorage.setItem('decodedToken', JSON.stringify(
                {
                    username: tokenDecoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
                    role: tokenDecoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
                }
            )
            );
            this.decodedToken = JSON.parse(localStorage.getItem('decodedToken') as any);
        }
    }

    onLogout() {
        localStorage.removeItem('userToken');
        this.token = localStorage.getItem('userToken');
        window.location.reload();
    }

    openLinkForm() {
        this.linkDialog.open(LinksComponent);
    }
    openLoginForm() {
        this.loginDialog.open(LoginComponent);
    }
    getDecodedAccessToken(token: string): any {
        try {
            return JSON.parse(window.atob(token.split('.')[1]));
        } catch (Error) {
            return null;
        }
    }
}