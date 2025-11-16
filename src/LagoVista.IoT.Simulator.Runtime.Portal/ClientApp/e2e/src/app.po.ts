// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 3b95f262fc3a200e815c4e20cb58e1a555f216ecbf3389998cb77d3b810b4928
// IndexVersion: 2
// --- END CODE INDEX META ---
import { browser, by, element } from 'protractor';

export class AppPage {
  navigateTo() {
    return browser.get('/');
  }

  getMainHeading() {
    return element(by.css('app-root h1')).getText();
  }
}
