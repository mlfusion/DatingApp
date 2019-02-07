
import { UserService } from './_services/user.service';
import { AuthGuard } from './_guards/auth.guard';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { NotificationService } from './_services/notification.service';
import { AuthService } from './_services/auth.service';
import { environment } from 'src/environments/environment';

import { MemberListResolver } from './_resolver/member-list-resolvers';
import { MemberDetailResolver } from './_resolver/member-detail-resolvers';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule } from '@angular/common/http';
import {FormsModule} from '@angular/forms';
import {BsDropdownModule, BsDatepickerModule, TabsModule } from 'ngx-bootstrap';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ValueComponent } from './value/value.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ValueModelComponent } from './value/valueModel/valueModel.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';


export function tokenGetters() {
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      ValueComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      ValueModelComponent,
      MemberListComponent,
      ListsComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      NgxGalleryModule,
      TabsModule.forRoot(),
      BsDropdownModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
         config: {
            tokenGetter: tokenGetters,
            whitelistedDomains: [environment.url.replace('http://', '')],
            blacklistedRoutes: [environment.apiUrl.replace('http://', '') + 'auth']
         }
      })
   ],
   providers: [
      AuthService,
      NotificationService,
      ErrorInterceptorProvider,
      AuthGuard,
      UserService,
      MemberDetailResolver,
      MemberListResolver,
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
