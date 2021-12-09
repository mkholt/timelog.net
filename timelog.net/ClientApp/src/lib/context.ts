import { createContext } from "react"
import { IContext } from "./IContext"

export const DefaultContext = {
	tasks: [],
	projects: [],
	entries: async () => []
} as IContext

const Context = createContext<IContext>(DefaultContext)

export const ContextProvider = Context.Provider
export default Context