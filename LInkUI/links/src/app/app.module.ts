import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LinksComponent } from './links/links.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { LoginComponent } from './login/login.component';
import { MatCardModule } from '@angular/material/card';
import { LoginService } from './services/login.service';
import { TokenInterceptorService } from './services/tokeninterceptor.service';
import { DatePipe } from '@angular/common';
import { InfoComponent } from './info/info.component';
import { AboutComponent } from './about/about.component';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    LinksComponent,
    LoginComponent,
    InfoComponent,
    AboutComponent,
    HomeComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    RouterModule,
    MatTableModule,
    ClipboardModule,
    MatCardModule,
  ],
  providers: [LoginService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorService,
      multi: true
    },
    DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule {

}
