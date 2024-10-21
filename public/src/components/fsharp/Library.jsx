import { printf, toConsole } from "./fable_modules/fable-library-js.4.22.0/String.js";

export function hello(name) {
    toConsole(printf("Hello %s"))(name);
}

