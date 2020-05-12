
export function wrapStore(store, module, idField) {
  const get = async (id) => {
    const { data } = await store.dispatch(`${module}/get`, { params: { id } })
    return data
  }
  const create = (data) => store.dispatch(`${module}/create`, { data })
  const update = (data) => store.dispatch(`${module}/update`, { params: { id: data[idField] }, data })
  const list = async (params) => {
    await store.dispatch(`${module}/list`, { params })
    return store.state.contacts
  }
  const remove = (id) => store.dispatch(`${module}/delete`, { params: { id } })
  return {
    get,
    create,
    update,
    list,
    remove
  }
}