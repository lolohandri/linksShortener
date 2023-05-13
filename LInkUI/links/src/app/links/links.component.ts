import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LinksService } from '../services/links.service';
import { DialogRef } from '@angular/cdk/dialog';
import { Router } from '@angular/router';

@Component({
    selector: 'app-links',
    templateUrl: './links.component.html',
    styleUrls: ['./links.component.css']
})
export class LinksComponent {
    @ViewChild('output', { static: true }) public resContainer: any;
    inputField: any;
    linkForm: FormGroup;
    shortUrl: string = '';
    buttonLabel: string = 'Copy';

    button: any;
    span: any;
    resultString: string = "";
    httpStatus: any;

    constructor(
        private _linkService: LinksService,
        private _linkForm: FormBuilder,
        private _dgRef: DialogRef<LinksComponent>,
        private router: Router,
    ) {
        this.linkForm = this._linkForm.group({
            url: '',
        });
    }

    onSubmit(): void {
        this._linkService.createLink(this.linkForm.value).subscribe({
            next: (val: any) => {
                this.shortUrl = val.shortLink;
                this.httpStatus = 200;
                this.router.navigate(['/home']);
            },
            error: (err: any) => {
                this.shortUrl = err.error;
                if (err.status == 401) {
                    this.shortUrl = "You should login to add new item";
                }
                else if (err.status == 403) {
                    this.shortUrl = "You don`t have access to add new item";
                }
                this.httpStatus = err.status;
            }
        })
    }

    onClear(): void {
        this.inputField = '';
        this.shortUrl = '';
    }

    onClose() {
        this._dgRef.close();
    }

    onCopy(): void {
        this.buttonLabel = 'Copied';
        setTimeout(() => {
            this.buttonLabel = 'Copy';
        }, 1000);
    }
}
