import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Link } from '../models/link.model';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class LinksService {
    baseUrl = 'https://localhost:7076/api/links';
    constructor(private _http: HttpClient) { }

    createLink(link: Link): Observable<Link> {
        return this._http.post<Link>(this.baseUrl, link);
    }

    getLinks(): Observable<Link[]> {
        return this._http.get<Link[]>(this.baseUrl);
    }

    deleteLink(id: number): Observable<any> {
        let httpheaders = new HttpHeaders().set('Content-Type', 'text/plain; charset=utf-8');
        return this._http.delete<any>(this.baseUrl + `/${id}`, { headers: httpheaders, responseType: 'text' as 'json' });
    }
}
