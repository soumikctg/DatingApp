import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { routes } from './app.routes';
import { provideToastr } from 'ngx-toastr';
import { errorInterceptor } from './_interceptors/error.interceptor';
import {jwtInterceptor} from "./_interceptors/jwt.interceptor";
import {loadingInterceptor} from "./_interceptors/loading.interceptor";
import {GALLERY_CONFIG, GalleryConfig} from "ng-gallery";
import {TimeagoFormatter, TimeagoModule} from "ngx-timeago";
import {ModalModule} from "ngx-bootstrap/modal";


export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient( withInterceptors([errorInterceptor, jwtInterceptor, loadingInterceptor])),
    provideAnimations(),
    provideToastr({
      timeOut: 3000,
      positionClass: 'toast-bottom-right',
    }),
    {
      provide: GALLERY_CONFIG,
      useValue: {
        autoHeight: false,
        imageSize: 'contain'
      } as GalleryConfig
    },
    importProvidersFrom(TimeagoModule.forRoot(), ModalModule.forRoot()),
  ],
};
